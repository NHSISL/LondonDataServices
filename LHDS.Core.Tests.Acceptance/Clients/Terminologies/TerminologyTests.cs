// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Ontologies;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Orchestrations.TerminologyMedata;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Orchestrations.TerminologyMetadata;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminologies
{
    [Collection(nameof(CoreTestCollection))]
    public partial class TerminologyTests
    {
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly Mock<IOntologyBroker> ontologyBrokerMock;
        private readonly ITerminologyArtifactService terminologyArtifactService;
        private readonly TerminologyMetadataConfiguration terminologyMetadataConfiguration;
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly ITerminologyMetadataOrchestrationService terminologyMetadataOrchestrationService;
        private readonly ITerminologyClient terminologyClient;
        private readonly DependencyBroker dependencyBroker;

        public TerminologyTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            this.ontologyBrokerMock = new Mock<IOntologyBroker>();

            terminologyMetadataConfiguration = new TerminologyMetadataConfiguration
            {
                ResourceURL = "/authoring/fhir/{{resourceType}}?_lastUpdated=ge{{datestamp}}" +
                    "&_name=dm+dCOMBINATION_PACK_IND&_elements=name,title,url,version,status&_count=10"
            };

            serviceCollection.AddTerminologyClientForAcceptance(this.dependencyBroker.Configuration);

            serviceCollection
                .AddTransient<IOntologyBroker>(serviceProvider => ontologyBrokerMock.Object)
                .AddTransient<ITerminologyMetadataOrchestrationService, TerminologyMetadataOrchestrationService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            documentProcessingService = serviceProvider.GetService<IDocumentProcessingService>();
            terminologyArtifactService = serviceProvider.GetService<ITerminologyArtifactService>();

            var terminologyMetadataOrchestrationService =
                serviceProvider.GetService<ITerminologyMetadataOrchestrationService>();

            terminologyClient = serviceProvider.GetService<ITerminologyClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
               new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static TerminologyPoll CreateRandomTerminologyPoll(string resourceType, DateTimeOffset lastPoll) =>
            CreateTerminologyPollFiller(resourceType, lastPoll).Create();

        private static Filler<TerminologyPoll> CreateTerminologyPollFiller(string resourceType, DateTimeOffset lastPoll)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<TerminologyPoll>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(terminologyMetadata => terminologyMetadata.ResourceType).Use(resourceType)
                .OnProperty(terminologyMetadata => terminologyMetadata.LastPoll).Use(lastPoll);

            return filler;
        }

        private static TerminologyArtifact CreateRandomUndownloadedTerminologyArtifact() =>
            CreateUndownloadedTerminologyArtifactFiller().Create();

        private static Filler<TerminologyArtifact> CreateUndownloadedTerminologyArtifactFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTimeOffset dateTimeOffset = DateTimeOffset.Now;
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsDownloaded).Use(false)
                .OnProperty(terminologyArtifact => terminologyArtifact.CreatedBy).Use(user)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedBy).Use(user);

            return filler;
        }

        private static OntologyAssets CreateRandomOntologyAssets()
        {
            OntologyAssets ontologyAssets = new OntologyAssets
            {
                Assets = CreateRandomOntologyAssetList(),
                NextPage = ""
            };

            return ontologyAssets;
        }

        private static List<OntologyAsset> CreateRandomOntologyAssetList() =>
            CreateOntologyFiller().Create(count: GetRandomNumber()).ToList();

        private static Filler<OntologyAsset> CreateOntologyFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();

            var filler = new Filler<OntologyAsset>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        private static List<dynamic> CreateRandomArtifactProperties(string artifactType)
        {
            return Enumerable.Range(1, GetRandomNumber())
                .Select(item =>
                {
                    return new
                    {
                        FullUrl = GetRandomString(),
                        ResourceType = artifactType,
                        Version = GetRandomString(),
                        Name = GetRandomString(),
                        Title = GetRandomString(),
                        Status = "active",
                        LastUpdated = GetRandomDateTimeOffset()
                    };
                })
                .ToList<dynamic>();
        }

        private static Bundle CreateCodeSystemBundleFromRandomData(
            List<dynamic> randomArtifactProperties,
            string nextPageUrl)
        {
            Bundle externalBundleResult = new Bundle
            {
                Id = Guid.NewGuid().ToString(),
                Type = Bundle.BundleType.Searchset,
                Total = randomArtifactProperties.Count,
                Link = new List<Bundle.LinkComponent>
                {
                    new Bundle.LinkComponent
                    {
                        Relation = "self",
                        Url = "http://localhost:5000/api/fhir/ValueSet"
                    },
                    new Bundle.LinkComponent
                    {
                        Relation = "next",
                        Url = nextPageUrl
                    }
                },
                Entry = new List<Bundle.EntryComponent>()
            };

            foreach (var item in randomArtifactProperties)
            {
                externalBundleResult.Entry.Add(
                    new Bundle.EntryComponent
                    {
                        FullUrl = item.FullUrl,
                        Resource = new CodeSystem
                        {
                            Meta = new Meta
                            {
                                LastUpdated = item.LastUpdated,
                            },

                            Version = item.Version,
                            Name = item.Name,
                            Title = item.Title,

                            Status = (PublicationStatus)Enum.Parse(
                                typeof(PublicationStatus), item.Status, ignoreCase: true),
                        }
                    });
            }

            return externalBundleResult;
        }
    }
}
