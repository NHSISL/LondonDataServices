// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Processings.TerminologyArtifacts;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingServiceTests
    {
        private readonly Mock<ITerminologyArtifactService> terminologyArtifactServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITerminologyArtifactProcessingService terminologyArtifactProcessingService;

        public TerminologyArtifactProcessingServiceTests()
        {
            this.terminologyArtifactServiceMock = new Mock<ITerminologyArtifactService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.terminologyArtifactProcessingService = new TerminologyArtifactProcessingService(
                terminologyArtifactService: this.terminologyArtifactServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static OntologyAsset CreateRandomOntologyAssets() =>
           CreateOntologyFiller().Create();

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
    }
}
