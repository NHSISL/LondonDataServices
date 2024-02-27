// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using LHDS.Core.Services.Processings.SecureDatas;
using LHDS.Core.Services.Processings.SubscriberAgreements;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestrationServiceTests
    {
        private readonly Mock<ISubscriberAgreementProcessingService> subscriberAgreementProcessingServiceMock;
        private readonly Mock<ISecureDataProcessingService> secureDataProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly ISubscriberCredentialOrchestration subscriberCredentialOrchestration;

        public SubscriberCredentialOrchestrationServiceTests()
        {
            this.subscriberAgreementProcessingServiceMock = new Mock<ISubscriberAgreementProcessingService>();
            this.secureDataProcessingServiceMock = new Mock<ISecureDataProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();

            this.subscriberCredentialOrchestration = new SubscriberCredentialOrchestration(
                subscriberAgreementProcessingService: subscriberAgreementProcessingServiceMock.Object,
                secureDataProcessingService: secureDataProcessingServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
           new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<SubscriberAgreement, bool>> SameSubscriberAgreementAs(
            SubscriberAgreement expectedSubscriberAgreement)
        {
            return actualSubscriberAgreement =>
                this.compareLogic.Compare(expectedSubscriberAgreement, actualSubscriberAgreement)
                    .AreEqual;
        }

        private Expression<Func<SubscriberCredential, bool>> SameSubscriberCredentialAs(
            SubscriberCredential expectedSubscriberCredential)
        {
            return actualSubscriberCredential =>
                this.compareLogic.Compare(expectedSubscriberCredential, actualSubscriberCredential)
                    .AreEqual;
        }

        private static dynamic CreateRandomDynamicSubscriberAgreementCredential()
        {
            Guid id = Guid.NewGuid();
            Guid supplierSharingAgreementGuid = Guid.NewGuid();
            DateTimeOffset randomDate = GetRandomDateTimeOffset();

            return new
            {
                Id = id,
                SupplierSharingAgreementShortName = GetRandomString(),
                SupplierSharingAgreementGuid = supplierSharingAgreementGuid,
                FtpUserName = GetRandomString(),
                FtpPassword = GetRandomString(),
                FtpPassPhrase = GetRandomString(),
                FtpPrivateKey = GetRandomString(),
                FtpPublicKey = GetRandomString(),
                GpgPassPhrase = GetRandomString(),
                GpgPrivateKey = GetRandomString(),
                GpgPublicKey = GetRandomString(),
                IsActive = false,
                LastPollStartDate = randomDate,
                LastPollEndDate = randomDate.AddMinutes(1)
            };
        }

        private static SubscriberCredential CreateSubscriberCredentialFromDynamic(dynamic credential)
        {
            SubscriberCredential randomSubscriberCredential = new SubscriberCredential
            {
                Id = credential.Id,
                SupplierSharingAgreementShortName = credential.SupplierSharingAgreementShortName,
                SupplierSharingAgreementGuid = credential.SupplierSharingAgreementGuid,
                FtpUserName = credential.FtpUserName,
                FtpPassword = credential.FtpPassword,
                FtpPassPhrase = credential.FtpPassPhrase,
                FtpPrivateKey = credential.FtpPrivateKey,
                FtpPublicKey = credential.FtpPublicKey,
                GpgPassPhrase = credential.GpgPassPhrase,
                GpgPrivateKey = credential.GpgPrivateKey,
                GpgPublicKey = credential.GpgPublicKey,
                IsActive = credential.IsActive,
                LastPollStartDate = credential.LastPollStartDate,
                LastPollEndDate = credential.LastPollEndDate
            };

            return randomSubscriberCredential;
        }

        private static SubscriberAgreement CreateSubscriberAgreementFromDynamic(dynamic credential)
        {
            SubscriberAgreement randomSubscriberAgreement = new SubscriberAgreement
            {
                Id = credential.Id,
                SupplierSharingAgreementShortName = credential.SupplierSharingAgreementShortName,
                SupplierSharingAgreementGuid = credential.SupplierSharingAgreementGuid,
                FtpUserName = credential.FtpUserName,
                FtpPublicKey = credential.FtpPublicKey,
                GpgPublicKey = credential.GpgPublicKey,
                IsActive = credential.IsActive,
                LastPollStartDate = credential.LastPollStartDate,
                LastPollEndDate = credential.LastPollEndDate
            };

            return randomSubscriberAgreement;
        }

        public static TheoryData SubscriberCredentialOrchestrationDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new SubscriberCredentialValidationException(
                    message: "Subscriber credential processing validation error occured, please try again",
                    innerException),

                new SubscriberCredentialProcessingDependencyValidationException(
                    message: "Subscriber credential processing dependency validation error occurred, please try again.",
                    innerException),
            };
        }

        public static TheoryData SubscriberCredentialOrchestrationDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new SubscriberCredentialProcessingDependencyException(
                    message: "Subscriber credential processing dependency error occurred, contact support.",
                    innerException),

                new SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, contact support.",
                    innerException),
            };
        }
    }
}
