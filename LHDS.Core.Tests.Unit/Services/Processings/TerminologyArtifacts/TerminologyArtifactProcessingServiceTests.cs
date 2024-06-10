// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Processings.TerminologyArtifacts;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingServiceTests
    {
        private readonly Mock<ITerminologyArtifactService> terminologyArtifactServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ITerminologyArtifactProcessingService terminologyArtifactProcessingService;

        public TerminologyArtifactProcessingServiceTests()
        {
            this.terminologyArtifactServiceMock = new Mock<ITerminologyArtifactService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.terminologyArtifactProcessingService = new TerminologyArtifactProcessingService(
                terminologyArtifactService: this.terminologyArtifactServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<TerminologyArtifact> CreateRandomTerminologyArtifacts()
        {
            return CreateTerminologyArtifactFiller()
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static TerminologyArtifact CreateRandomTerminologyArtifact() =>
            CreateTerminologyArtifactFiller().Create();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Filler<TerminologyArtifact> CreateTerminologyArtifactFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnProperty(terminologyArtifact => terminologyArtifact.IsDownloaded).Use(true)
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new TerminologyArtifactValidationException(
                    message: "Terminology artifact validation error occurred, please try again.", innerException),

                new TerminologyArtifactDependencyValidationException(
                    message: "Terminology artifact dependency validation error occurred, please try again.", innerException)
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new TerminologyArtifactDependencyException(
                    message: "Terminology artifact dependency error occurred, please try again.", innerException),

                new TerminologyArtifactServiceException(
                    message: "Terminology artifact service error occurred, please try again.", innerException)
            };
        }
    }
}
