// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Text;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Foundations.CryptographicKeys;
using LHDS.Core.Services.Processings.CryptographicKeys;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.CryptographicKeys
{
    public partial class CryptographicKeyProcessingServiceTests
    {
        private readonly Mock<ICryptographyKeyService> cryptographyKeyServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly ICryptographyKeyProcessingService cryptographyKeyProcessingService;

        public CryptographicKeyProcessingServiceTests()
        {
            cryptographyKeyServiceMock = new Mock<ICryptographyKeyService>();
            cryptographyKeyProcessingService = new CryptographyKeyProcessingService(
                cryptographyKeyService: cryptographyKeyServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

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

        private static CryptographicKey CreateRandomCryptographicKey()
        {
            return new CryptographicKey
            {
                PrivateKey = GetRandomString(),
                PublicKey = GetRandomString(),
                Passphrase = GetRandomString()
            };
        }

        private string ConvertToBase64(string value)
        {
            byte[] byteValue = Encoding.UTF8.GetBytes(value);

            return Convert.ToBase64String(byteValue);
        }

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new CryptographyKeyValidationException(
                    message: "Cryptography key validation errors occurred, please try again.", innerException),

                new CryptographyKeyDependencyValidationException(
                    message: "Cryptography key dependency validation occurred, please try again.", innerException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new CryptographyKeyDependencyException(
                    message: "Cryptography key dependency errors occurred, please try again.", innerException),

                new CryptographyKeyServiceException(
                    message : "Cryptography key service error occurred, please contact support.", innerException)
            };
        }
    }
}
