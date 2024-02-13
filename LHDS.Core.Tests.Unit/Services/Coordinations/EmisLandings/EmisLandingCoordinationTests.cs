// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Downloads;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        private readonly Mock<ISubscriberCredentialOrchestration> subscriberCredentialOrchestrationMock;
        private readonly Mock<IEmisLandingOrchestrationService> emisLandingExtractionOrchestrationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IEmisLandingCoordinationService emisLandingCoordinationService;

        public EmisLandingCoordinationServiceTests()
        {
            this.subscriberCredentialOrchestrationMock = new Mock<ISubscriberCredentialOrchestration>();
            this.emisLandingExtractionOrchestrationServiceMock = new Mock<IEmisLandingOrchestrationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.emisLandingCoordinationService = new EmisLandingCoordinationService(
                subscriberCredentialOrchestration: subscriberCredentialOrchestrationMock.Object,
                emisLandingOrchestrationService: emisLandingExtractionOrchestrationServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        private static List<Guid> CreateRandomActiveSubscriberAgreementIds(int number)
        {
            return Enumerable.Range(0, number)
                .Select(_ => Guid.NewGuid())
                .ToList();
        }

        private static List<string> CreateRandomLandingPaths(int number)
        {
            return Enumerable.Range(0, number)
                .Select(_ => GetRandomString())
                .ToList();
        }

        public static List<SubscriberCredential> CreateRandomSubscriberCredentials(
            List<Guid> subscriberAgreementIds)
        {
            List<SubscriberCredential> subscriberCredentials = new List<SubscriberCredential>();

            foreach (Guid subscriberAgreementId in subscriberAgreementIds)
            {
                subscriberCredentials.Add(CreateRandomSubscriberCredentialsFiller(subscriberAgreementId).Create());
            }

            return subscriberCredentials;
        }

        public static Filler<SubscriberCredential> CreateRandomSubscriberCredentialsFiller(Guid subscriberAgreementId)
        {
            DateTimeOffset dateTimeOffset = new DateTimeRange(earliestDate: new DateTime()).GetValue();
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(subscriberCredential => subscriberCredential.Id).Use(() => subscriberAgreementId);

            return filler;
        }

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new SubscriberCredentialValidationOrchestrationException(
                    message: "Subscriber credential orchestration validation error occured, please try again",
                    innerException),

                //new SubscriberCredentialOrchestrationDependencyValidationException(
                //    message: "Subscriber credential orchestration dependency validation error occurred, " +
                //        "please try again.",

                //    innerException),

                //new EmisLandingOrchestrationValidationException(
                //    message: "EMIS landing orchestration validation error occured, please try again",
                //    innerException),

                //new EmisLandingOrchestrationDependencyValidationException(
                //    message: "EMIS landing orchestration dependency validation error occurred, " +
                //        "please try again.",

                //    innerException),
            };
        }
    }
}
