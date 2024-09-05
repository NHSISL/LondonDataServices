// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Extensions.Exceptions;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using LHDS.Core.Models.Orchestrations.TppLandings.Exceptions;
using LHDS.Core.Services.Coordinations.TppLandings;
using LHDS.Core.Services.Orchestrations.Ingress;
using LHDS.Core.Services.Orchestrations.TppLandings;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class TppLandingsCoordinationTests
    {
        private readonly Mock<ITppLandingOrchestrationService> tppLandingOrchestrationServiceMock;
        private readonly Mock<IIngressOrchestrationService> ingressOrchestrationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly ITppLandingCoordinationService tppLandingCoordinationService;

        public TppLandingsCoordinationTests()
        {
            this.tppLandingOrchestrationServiceMock = new Mock<ITppLandingOrchestrationService>();
            this.ingressOrchestrationServiceMock = new Mock<IIngressOrchestrationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.tppLandingCoordinationService = new TppLandingCoordinationService(
                tppOrchestrationService: tppLandingOrchestrationServiceMock.Object,
                ingressOrchestrationService: ingressOrchestrationServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
          new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
         actualException => actualException.SameExceptionAs(expectedException);

        private static Expression<Func<Xeption, bool>> IsSameExceptionAs(Xeption expectedException) =>
            actualException => actualException.IsSameExceptionAs(expectedException);

        public static TheoryData<Xeption> TppDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new TppLandingOrchestrationValidationException(
                    message: "TPP landing orchestration validation errors occured, please try again.",
                    innerException),

                new TppLandingOrchestrationDependencyValidationException(
                    message: "TPP landing orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException),

                new IngressOrchestrationValidationException(
                    message: "Ingres orchestration validation errors occured, please try again.",
                    innerException),

                new IngressOrchestrationDependencyValidationException(
                    message: "Ingres orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException)
            };
        }

        public static TheoryData<Xeption> TppDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new TppLandingOrchestrationDependencyException(
                    message: "TPP landing orchestration dependency error occurred, fix the errors and try again.",
                    innerException),

                new TppLandingOrchestrationServiceException(
                    message: "TPP landing orchestration service error occurred, please contact support.",
                    innerException),

                new IngressOrchestrationDependencyException(
                    message: "Ingres orchestration dependency error occurred, fix the errors and try again.",
                    innerException),

                new IngressOrchestrationServiceException(
                    message: "Ingres orchestration service error occurred, please contact support.",
                    innerException)
            };
        }
    }
}
