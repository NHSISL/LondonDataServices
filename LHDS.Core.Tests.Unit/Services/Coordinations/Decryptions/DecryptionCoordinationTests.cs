// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Extensions.Exceptions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Coordinations.Decryptions;
using LHDS.Core.Services.Orchestrations.Decryptions;
using LHDS.Core.Services.Orchestrations.Ingress;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationServiceTests
    {
        private readonly Mock<ISubscriberCredentialOrchestration> subscriberCredentialOrchestrationMock;
        private readonly Mock<IDecryptionOrchestrationService> decryptionOrchestrationServiceMock;
        private readonly Mock<IIngressOrchestrationService> ingressOrchestrationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IDecryptionCoordinationService decryptionCoordinationService;

        public DecryptionCoordinationServiceTests()
        {
            this.subscriberCredentialOrchestrationMock = new Mock<ISubscriberCredentialOrchestration>();
            this.decryptionOrchestrationServiceMock = new Mock<IDecryptionOrchestrationService>();
            this.ingressOrchestrationServiceMock = new Mock<IIngressOrchestrationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.decryptionCoordinationService = new DecryptionCoordinationService(
                subscriberCredentialOrchestration: subscriberCredentialOrchestrationMock.Object,
                decryptionOrchestrationService: decryptionOrchestrationServiceMock.Object,
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

        public static SubscriberCredential CreateRandomSubscriberCredential(Guid subscriberAgreementId)
        {
            DateTimeOffset dateTimeOffset = new DateTimeRange(earliestDate: new DateTime()).GetValue();
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(subscriberCredential => subscriberCredential.Id).Use(() => subscriberAgreementId);

            return filler.Create();
        }

        private static string CreateRandomFilePath(Guid identifier)
        {
            return $"{GetRandomString()}/{GetRandomString()}" +
                $"/{identifier}/0122235/{GetRandomNumber}" +
                $"_{GetRandomString()}_{GetRandomString()}" +
                $"_{GetRandomNumber()}_{identifier}.csv.gpg;";
        }

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new SubscriberCredentialValidationOrchestrationException(
                    message: "Subscriber credential orchestration validation error occured, please try again",
                    innerException),

                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: "Subscriber credential orchestration dependency validation error occurred, " +
                        "please try again.",
                    innerException),

                new DecryptionOrchestrationValidationException(
                    message: "Decryption orchestration validation error occured, please try again.",
                    innerException),

                new DecryptionOrchestrationDependencyValidationException(
                    message: "Decryption orchestration dependency validation error occurred, " +
                        "please try again.",
                    innerException),

                new IngressOrchestrationValidationException(
                    message: "Ingres orchestration validation error occured, please try again.",
                    innerException),

                new IngressOrchestrationDependencyValidationException(
                    message: "Ingres orchestration dependency validation error occurred, " +
                        "please try again.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new SubscriberCredentialDependencyOrchestrationException(
                    message: "Subscriber credential orchestration dependency error occured, please try again.",
                    innerException),

                new SubscriberCredentialOrchestrationServiceException(
                    message: "Subscriber credential orchestration service error occurred, please contact support.",
                    innerException),

                new DecryptionOrchestrationDependencyException(
                    message: "Decryption orchestration dependency error occured, please try again.",
                    innerException),

                new DecryptionOrchestrationServiceException(
                    message: "Decryption orchestration service error occurred, please contact support.",
                    innerException),

                new IngressOrchestrationDependencyException(
                    message: "Ingres orchestration dependency error occured, please try again.",
                    innerException),

                new IngressOrchestrationServiceException(
                    message: "Ingres orchestration service error occurred, please contact support.",
                    innerException)
            };
        }

        private static SubscriberCredential CreateRandomSubscriberCredential() =>
            CreateSubscriberCredentialFiller().Create();

        private static Filler<SubscriberCredential> CreateSubscriberCredentialFiller()
        {
            var filler = new Filler<SubscriberCredential>();
            string user = Guid.NewGuid().ToString();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy).Use(user)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedBy).Use(user);

            return filler;
        }
    }
}
