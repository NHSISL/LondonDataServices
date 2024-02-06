using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.Addresses;
using Xunit;
using Azure.Security.KeyVault.Secrets;
using LHDS.Core.Models.Foundations.SecureData;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SecureDatas
{
    public partial class SecureDataServiceTests
    {
        [Fact]
        public async Task ShouldAddOrModifySecureDataAsync()
        {
            // given
            dynamic randomSecret = CreateRandomSecret();
            KeyVaultSecret inputKeyVaultSecret = CreateKeyVaultSecretFromRandomSecret(randomSecret);
            KeyVaultSecret outputKeyVaultSecret = inputKeyVaultSecret.DeepClone();
            SecureData inputSecureData = CreateSecureDataFromRandomSecret(randomSecret);
            SecureData expectedSecureData = inputSecureData.DeepClone();

            this.secureDataBrokerMock.Setup(broker =>
                broker.CreateOrUpdateKeyVaultSecretAsync(It.Is(SameKeyVaultSecretAs(inputKeyVaultSecret))))
                    .ReturnsAsync(outputKeyVaultSecret);

            // when
            SecureData actualSecureData = await this.secureDataService
                .AddOrModifySecureData(inputSecureData);

            // then
            actualSecureData.Should().BeEquivalentTo(expectedSecureData);

            this.secureDataBrokerMock.Verify(broker =>
                broker.CreateOrUpdateKeyVaultSecretAsync(It.Is(SameKeyVaultSecretAs(inputKeyVaultSecret))),
                    Times.Once());

            this.secureDataBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}