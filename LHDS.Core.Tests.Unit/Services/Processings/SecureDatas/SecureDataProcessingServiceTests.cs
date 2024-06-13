// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Foundations.SecureDatas;
using LHDS.Core.Services.Processings.SecureDatas;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingServiceTests
    {
        private readonly Mock<ISecureDataService> secureDataServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ISecureDataProcessingService secureDataProcessingService;
        private readonly ICompareLogic compareLogic;

        public SecureDataProcessingServiceTests()
        {
            this.secureDataServiceMock = new Mock<ISecureDataService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            compareLogic = new CompareLogic();

            this.secureDataProcessingService = new SecureDataProcessingService(
                secureDataService: this.secureDataServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static List<string> GetRandomProperties()
        {
            int randomNumber = GetRandomNumber();
            List<string> properties = new List<string>();

            for (int i = 0; i < randomNumber; i++)
            {
                properties.Add(GetRandomString());
            }

            return properties;
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private Expression<Func<SecureData, bool>> SameSecureDataAs(SecureData expectedSecureData)
        {
            return actualSecureData =>
                this.compareLogic.Compare(expectedSecureData, actualSecureData)
                    .AreEqual;
        }

        private static dynamic CreateRandomDynamicSharingAgreementCredential()
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
                LastPollEndDate = randomDate.AddMinutes(1),
            };
        }

        private static SecureData CreateSecretDataFromDynamic(dynamic credential, string property)
        {
            string secretName = $"{credential.Id}-{property}";
            string secretValue = GetDynamicPropertyValue(credential, property);

            SecureData secureData = new SecureData
            {
                Name = secretName,
                Value = secretValue
            };

            return secureData;
        }

        private static string GetDynamicPropertyValue(dynamic obj, string propertyName)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                object value = propertyInfo.GetValue(obj);
                return value?.ToString() ?? string.Empty;
            }
            else
            {
                throw new ArgumentException($"Property '{propertyName}' not found on object.");
            }
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

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new SecureDataValidationException(
                    message: "Secure data validation errors occurred, please try again.", innerException),

                new SecureDataDependencyValidationException(
                    message: "Secure data dependency validation occurred, please try again.", innerException)
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new SecureDataDependencyException(
                    message: "Secure data validation errors occurred, please try again.", innerException),

                new SecureDataServiceException(
                    message : "Secure data service error occurred, please contact support.", innerException)
            };
        }
    }
}