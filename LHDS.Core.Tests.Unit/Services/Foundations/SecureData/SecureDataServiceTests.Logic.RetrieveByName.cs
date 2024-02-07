// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SecureData;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SecureDatas
{
    public partial class SecureDataServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveSecureDataByNameAsync()
        {
            // given
            dynamic randomSecret = CreateRandomSecret();
            string secretName = randomSecret.Name;
            KeyVaultSecret outputKeyVaultSecret = CreateKeyVaultSecretFromRandomSecret(randomSecret);
            SecureData expectedSecureData = CreateSecureDataFromRandomSecret(randomSecret);

            this.secureDataBrokerMock.Setup(broker =>
                broker.GetKeyVaultSecretAsync(secretName))
                    .ReturnsAsync(outputKeyVaultSecret);

            // when
            SecureData actualSecureData = await this.secureDataService.RetrieveSecretDataByNameAsync(secretName);

            // then
            actualSecureData.Should().BeEquivalentTo(expectedSecureData);

            this.secureDataBrokerMock.Verify(broker =>
                broker.GetKeyVaultSecretAsync(secretName),
                    Times.Once());

            this.secureDataBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}