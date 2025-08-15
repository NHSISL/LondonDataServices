// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using LHDS.Core.Services.Processings.CryptographicKeys;
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
        private readonly Mock<ICryptographyKeyProcessingService> cryptographyKeyProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly ISubscriberCredentialOrchestration subscriberCredentialOrchestration;

        public SubscriberCredentialOrchestrationServiceTests()
        {
            this.subscriberAgreementProcessingServiceMock = new Mock<ISubscriberAgreementProcessingService>();
            this.secureDataProcessingServiceMock = new Mock<ISecureDataProcessingService>();
            this.cryptographyKeyProcessingServiceMock = new Mock<ICryptographyKeyProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();

            this.subscriberCredentialOrchestration = new SubscriberCredentialOrchestration(
                subscriberAgreementProcessingService: subscriberAgreementProcessingServiceMock.Object,
                secureDataProcessingService: secureDataProcessingServiceMock.Object,
                cryptographyKeyProcessingService: cryptographyKeyProcessingServiceMock.Object,
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

        private static List<dynamic> CreateRandomDynamicSubscriberAgreementCredentials()
        {
            return Enumerable.Range(1, 1)
                .Select(item => CreateRandomDynamicSubscriberAgreementCredential(date: GetRandomDateTimeOffset()))
                    .ToList();
        }

        private static IQueryable<SubscriberCredential> CreateSubscriberCredentialsFromDynamic(
            List<dynamic> credentials)
        {
            List<SubscriberCredential> subscriberCredentials = new List<SubscriberCredential>();

            foreach (var credential in credentials)
            {
                SubscriberCredential subscriberCredential = CreateSubscriberCredentialFromDynamic(credential);
                subscriberCredential.FtpPassword = null;
                subscriberCredential.FtpPrivateKey = null;
                subscriberCredential.FtpPassPhrase = null;
                subscriberCredential.GpgPassPhrase = null;
                subscriberCredential.GpgPrivateKey = null;
                subscriberCredentials.Add(subscriberCredential);
            }

            return subscriberCredentials.AsQueryable();
        }

        private static IQueryable<SubscriberAgreement> CreateSubscriberAgreementsFromDynamic(
            List<dynamic> credentials)
        {
            List<SubscriberAgreement> subscriberAgreements = new List<SubscriberAgreement>();

            foreach (dynamic credential in credentials)
            {
                SubscriberAgreement subscriberAgreement = CreateSubscriberAgreementFromDynamic(credential);

                subscriberAgreements.Add(subscriberAgreement);
            }

            return subscriberAgreements.AsQueryable();
        }

        private static dynamic CreateRandomDynamicSubscriberAgreementCredential(
            DateTimeOffset date,
            Guid id = default(Guid))
        {
            if (id == Guid.Empty)
            {
                id = Guid.NewGuid();
            }

            Guid supplierSharingAgreementGuid = Guid.NewGuid();

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
                LastPollStartDate = date,
                LastPollEndDate = date.AddMinutes(1),
                CreatedBy = GetRandomString(),
                UpdatedBy = GetRandomString(),
                UpdatedDate = date,
                CreatedDate = date
            };
        }

        private static SubscriberCredential CreateSubscriberCredentialFromDynamic(
            dynamic credential,
            bool blankKeys = false)
        {
            SubscriberCredential randomSubscriberCredential = new SubscriberCredential
            {
                Id = credential.Id,
                SupplierSharingAgreementShortName = credential.SupplierSharingAgreementShortName,
                SupplierSharingAgreementGuid = credential.SupplierSharingAgreementGuid,
                FtpUserName = credential.FtpUserName,
                FtpPassword = blankKeys ? null : credential.FtpPassword,
                FtpPassPhrase = blankKeys ? null : credential.FtpPassPhrase,
                FtpPrivateKey = blankKeys ? null : credential.FtpPrivateKey,
                FtpPublicKey = credential.FtpPublicKey,
                GpgPassPhrase = blankKeys ? null : credential.GpgPassPhrase,
                GpgPrivateKey = blankKeys ? null : credential.GpgPrivateKey,
                GpgPublicKey = credential.GpgPublicKey,
                IsActive = credential.IsActive,
                LastPollStartDate = credential.LastPollStartDate,
                LastPollEndDate = credential.LastPollEndDate,
                CreatedBy = credential.CreatedBy,
                UpdatedBy = credential.UpdatedBy,
                UpdatedDate = credential.UpdatedDate,
                CreatedDate = credential.CreatedDate
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
                LastPollEndDate = credential.LastPollEndDate,
                CreatedBy = credential.CreatedBy,
                UpdatedBy = credential.UpdatedBy,
                UpdatedDate = credential.UpdatedDate,
                CreatedDate = credential.CreatedDate
            };

            return randomSubscriberAgreement;
        }

        private static SubscriberAgreement CreateSubscriberAgreementFromSubscriberCredential(
            SubscriberCredential subscriberCredential)
        {
            SubscriberAgreement subscriberAgreement = new SubscriberAgreement
            {
                Id = subscriberCredential.Id,
                SupplierSharingAgreementShortName = subscriberCredential.SupplierSharingAgreementShortName,
                SupplierSharingAgreementGuid = subscriberCredential.SupplierSharingAgreementGuid,
                FtpUserName = subscriberCredential.FtpUserName,
                FtpPublicKey = subscriberCredential.FtpPublicKey,
                GpgPublicKey = subscriberCredential.GpgPublicKey,
                IsActive = subscriberCredential.IsActive,
                LastPollStartDate = subscriberCredential.LastPollStartDate,
                LastPollEndDate = subscriberCredential.LastPollEndDate,
                CreatedBy = subscriberCredential.CreatedBy,
                UpdatedBy = subscriberCredential.UpdatedBy,
                UpdatedDate = subscriberCredential.UpdatedDate,
                CreatedDate = subscriberCredential.CreatedDate
            };

            return subscriberAgreement;
        }

        private static List<SubscriberAgreement> CreateRandomSubscriberAgreements(Guid supplierId)
        {
            return CreateSubscriberAgreementFiller(dateTimeOffset: GetRandomDateTimeOffset(), supplierId)
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static SubscriberAgreement CreateRandomSubscriberAgreement(Guid supplierId) =>
            CreateSubscriberAgreementFiller(dateTimeOffset: GetRandomDateTimeOffset(), supplierId).Create();

        private static SubscriberAgreement CreateRandomSubscriberAgreement(
            DateTimeOffset dateTimeOffset,
            Guid supplierId) =>
                CreateSubscriberAgreementFiller(dateTimeOffset, supplierId).Create();

        private static Filler<SubscriberAgreement> CreateSubscriberAgreementFiller(
            DateTimeOffset dateTimeOffset,
            Guid supplierId)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SubscriberAgreement>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(subscriberAgreement => subscriberAgreement.SupplierId).Use(supplierId)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(user)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedBy).Use(user)
                .OnProperty(subscriberAgreement => subscriberAgreement.IngestionTrackings).IgnoreIt();

            return filler;
        }

        public static TheoryData<Xeption> SubscriberCredentialOrchestrationDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                 new SubscriberAgreementProcessingValidationException(
                    message: "Subscriber agreement processing validation error occurred, please try again",
                    innerException),

                new SubscriberAgreementProcessingDependencyValidationException(
                    message: "Subscriber agreement processing dependency validation error occurred, please try again.",
                    innerException),

                new SubscriberCredentialValidationException(
                    message: "Subscriber credential processing validation error occurred, please try again",
                    innerException),

                new SubscriberCredentialProcessingDependencyValidationException(
                    message: "Subscriber credential processing dependency validation error occurred, please try again.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> SubscriberCredentialOrchestrationDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new SubscriberAgreementProcessingDependencyException(
                    message: "Subscriber agreement processing dependency error occurred, please contact support.",
                    innerException),

                new SubscriberAgreementProcessingServiceException(
                    message: "Subscriber agreement processing service error occurred, please contact support.",
                    innerException),

                new SubscriberCredentialProcessingDependencyException(
                    message: "Subscriber credential processing dependency error occurred, please contact support.",
                    innerException),

                new SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, please contact support.",
                    innerException),
            };
        }
    }
}
