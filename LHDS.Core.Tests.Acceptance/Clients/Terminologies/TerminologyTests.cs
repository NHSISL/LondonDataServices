// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Ontologies;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Orchestrations.TerminologyMedata;
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
        private readonly Mock<OntologyBroker> ontologyBrokerMock;
        private readonly TerminologyMetadataConfiguration terminologyMetadataConfiguration;
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

            Mock<IOntologyBroker> ontologyBrokerMock = new Mock<IOntologyBroker>();

            terminologyMetadataConfiguration = new TerminologyMetadataConfiguration
            {
                ResourceURL = "/authoring/fhir/{{resourceType}}?_lastUpdated=ge{{datestamp}}" +
                    "&_name=dm+dCOMBINATION_PACK_IND&_elements=name,title,url,version,status&_count=10"
            };

            serviceCollection
                .AddTransient<IOntologyBroker>(serviceProvider => ontologyBrokerMock.Object);

            serviceCollection.AddTerminologyClientForAcceptance(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
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
    }
}
