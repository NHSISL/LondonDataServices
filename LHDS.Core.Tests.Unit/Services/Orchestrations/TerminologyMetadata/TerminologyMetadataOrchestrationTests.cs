// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
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
                dateTimeBroker: dateTimeBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
                new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
          new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}