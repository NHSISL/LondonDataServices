// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<IResolvedAddressProcessingService> resolvedAddressProcessingServiceMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IResolvedAddressOrchestrationService resolvedAddressOrchestrationService;

        public ResolvedAddressOrchestrationTests()
        {
            this.documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            this.resolvedAddressProcessingServiceMock = new Mock<IResolvedAddressProcessingService>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.resolvedAddressOrchestrationService = new ResolvedAddressOrchestrationService(
                documentProcessingService: this.documentProcessingServiceMock.Object,
                resolvedAddressProcessingService: this.resolvedAddressProcessingServiceMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentProcessingValidationException(
                    message: "Document processing validation error occured, please try again",
                    innerException),

                new DocumentProcessingDependencyValidationException(
                    message: "Document processing dependency validation error occurred, please try again.",
                    innerException),

                new ResolvedAddressProcessingValidationException(
                    message: "Resolved address processing validation error occured, please try again",
                    innerException),

                new ResolvedAddressProcessingDependencyValidationException(
                    message: "Resolved address processing dependency validation error occurred, please try again.",
                    innerException),
            };
        }

        public static TheoryData DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentProcessingDependencyException(
                    message: "Document processing dependency error occurred, contact support.",
                    innerException),

                new DocumentProcessingServiceException(
                    message: "Document processing service error occurred, contact support.",
                    innerException),

                new ResolvedAddressProcessingDependencyException(
                    message: "Resolved address processing dependency error occurred, contact support.",
                    innerException),

                new ResolvedAddressProcessingServiceException(
                    message: "Resolved address processing service error occurred, contact support.",
                    innerException),
            };
        }

    }
}
