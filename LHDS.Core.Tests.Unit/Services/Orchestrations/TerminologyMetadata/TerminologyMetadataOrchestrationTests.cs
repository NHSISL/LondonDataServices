// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Orchestrations.Ontologies;
using LHDS.Core.Services.Orchestrations.TerminologyMetadata;
using LHDS.Core.Services.Processings.Ontologies;
using LHDS.Core.Services.Processings.TerminologyArtifacts;
using LHDS.Core.Services.Processings.TerminologyPolls;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMetadataOrchestrationTests
    {
        private readonly Mock<ITerminologyPollProcessingService> terminologyPollProcessingServiceMock;
        private readonly Mock<ITerminologyArtifactProcessingService> terminologyArtifactProcessingServiceMock;
        private readonly Mock<IOntologyProcessingService> ontologyProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly OntologyConfiguration ontologyConfiguration;
        private readonly ITerminologyMetadataOrchestrationService terminologyMetadataOrchestrationService;
        private readonly ICompareLogic compareLogic;

        public TerminologyMetadataOrchestrationTests()
        {
            terminologyPollProcessingServiceMock = new Mock<ITerminologyPollProcessingService>();
            terminologyArtifactProcessingServiceMock = new Mock<ITerminologyArtifactProcessingService>();
            ontologyProcessingServiceMock = new Mock<IOntologyProcessingService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            compareLogic = new CompareLogic();

            terminologyMetadataOrchestrationService = new TerminologyMetadataOrchestrationService(
                terminologyPollProcessingService: terminologyPollProcessingServiceMock.Object,
                terminologyArtifactProcessingService: terminologyArtifactProcessingServiceMock.Object,
                ontologyProcessingService: ontologyProcessingServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,

            ontologyConfiguration = new OntologyConfiguration(
                resourceURL: $"{{terminology-server}}/{resourceType}?_lastUpdated=ge{{datestamp}}" +
                        "&_name=dm+dCOMBINATION_PACK_IND&_elements=name,title,url,version,status&_count=10"));
        }

        private static int GetRandomNumber() =>
                new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
          new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IQueryable<TerminologyPoll> CreateRandomTerminologyPolls(string resourceType)
        {
            return CreateTerminologyPollFiller(resourceType)
                .Create(count: 1)
                    .AsQueryable();
        }

        private static TerminologyPoll CreateRandomTerminologyPoll(string resourceType) =>
            CreateTerminologyPollFiller(resourceType).Create();

        private static Filler<TerminologyPoll> CreateTerminologyPollFiller(string resourceType)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<TerminologyPoll>();

            filler.Setup()
                .OnProperty(terminologyMetadata => terminologyMetadata.ResourceType).Use(resourceType)
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        private static List<OntologyAsset> CreateRandomOntologyAssets() =>
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

        private static dynamic CreateRandomArtifactProperties(string resourceType)
        {
            string user = GetRandomString();
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            return new
            {
                FullUrl = GetRandomString(),
                ResourceType = resourceType,
                Version = GetRandomString(),
                Name = GetRandomString(),
                Title = GetRandomString(),
                Status = "active",
                LastUpdated = dateTimeOffset,
                IsCore = false,
                IsDownloaded = true,
                CreatedBy = user,
                UpdateBy = user,
                UpdatedDate = dateTimeOffset,
                CreatedDate = dateTimeOffset
            };
        }

        private static OntologyAsset CreateOntologyArtiFactFromRandomData(
            dynamic randomArtifactProperties,
            string nextPageUrl)
        {
            var ontologyAssets = new OntologyAssets
            {
                Assets = new List<OntologyAsset>(),
                NextPage = nextPageUrl
            };


            ontologyAssets.Assets.Add(
                new OntologyAsset
                {
                    FullUrl = item.FullUrl,
                    ResourceType = item.ResourceType,
                    Version = item.Version,
                    Name = item.Name,
                    Title = item.Title,
                    Status = item.Status,
                    LastUpdated = item.LastUpdated
                });

            return ontologyAssets;
        }

        private static TerminologyArtifact CreateTerminologyArtiFactFromRandomData(
            dynamic randomArtifactProperties)
        {
            TerminologyArtifact terminologyArtifact =
                new TerminologyArtifact
                {
                    FullUrl = randomArtifactProperties.FullUrl,
                    ResourceType = randomArtifactProperties.ResourceType,
                    Version = randomArtifactProperties.Version,
                    Name = randomArtifactProperties.Name,
                    Title = randomArtifactProperties.Title,
                    Status = randomArtifactProperties.Status,
                    LastUpdated = randomArtifactProperties.LastUpdated,
                    IsCore = randomArtifactProperties.IsCore,
                    IsDownloaded = randomArtifactProperties.IsDownloaded,
                    CreatedBy = randomArtifactProperties.LastUpdated,
                    UpdatedBy = randomArtifactProperties.UpdatedBy,
                    UpdatedDate = randomArtifactProperties.UpdatedDate,
                    CreatedDate = randomArtifactProperties.CreatedDate
                });

            return terminologyArtifact;
        }
    }
}