// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Foundations.CryptographicKeys;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

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
                Base64PrivateKey = GetRandomString(),
                Base64PublicKey = GetRandomString(),
                Passphrase = GetRandomString()
            };
        }
    }
}
