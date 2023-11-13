// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
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

        private static IQueryable<TerminologyArtifact> CreateRandomTerminologyArtifacts()
        {
            return CreateTerminologyArtifactFiller()
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static TerminologyArtifact CreateRandomTerminologyArtifact() =>
            CreateTerminologyArtifactFiller().Create();

        private static Filler<TerminologyArtifact> CreateTerminologyArtifactFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }
    }
}
