// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using Azure.Security.KeyVault.Secrets;
using LHDS.Core.Brokers.KeyVaults;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Services.Foundations.SecureDatas;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SecureDatas
{
    public partial class SecureDataServiceTests
    {
        private readonly Mock<ISecureDataBroker> secureDataBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISecureDataService secureDataService;

        public SecureDataServiceTests()
        {
            this.secureDataBrokerMock = new Mock<ISecureDataBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.secureDataService = new SecureDataService(
                secureDataBroker: this.secureDataBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static dynamic CreateRandomSecret()
        {
            return new
            {
                Name = GetRandomString(),
                Value = GetRandomString()
            };
        }

        private static KeyVaultSecret CreateKeyVaultSecretFromRandomSecret(dynamic secret)
        {
            KeyVaultSecret randomKeyVaultSecret = new KeyVaultSecret(name: secret.Name, value: secret.Value);

            return randomKeyVaultSecret;
        }

        private static SecureData CreateSecureDataFromRandomSecret(dynamic secret)
        {
            SecureData randomSecureData = new SecureData
            {
                Name = secret.Name,
                Value = secret.Value
            };

            return randomSecureData;
        }
    }
}