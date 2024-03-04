// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LHDS.Core.Brokers.CryptographyKeys;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using LHDS.Core.Services.Foundations.CryptographicKeys;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyServiceTests
    {
        private readonly Mock<ICryptographyKeyBroker> cryptographyKeyBroker;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICryptographyKeyService cryptographyKeyService;

        public CryptographyKeyServiceTests()
        {
            this.cryptographyKeyBroker = new Mock<ICryptographyKeyBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.cryptographyKeyService = new CryptographyKeyService(
                cryptographyKeyBrokers: new List<ICryptographyKeyBroker> { this.cryptographyKeyBroker.Object },
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static CryptographicKey GenerateRandomCryptographicKey()
        {
            return new CryptographicKey
            {
                Base64PublicKey = GetRandomString(),
                Base64PrivateKey = GetRandomString(),
                Passphrase = GetRandomString()
            };
        }
    }
}

