// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SecureData;
using Moq;
using Xunit;

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

            this.keyVaultSecretBrokerMock.Setup(broker =>
                broker.CreateOrUpdateKeyVaultSecretAsync(It.Is(SameKeyVaultSecretAs(inputKeyVaultSecret))))
                    .ReturnsAsync(outputKeyVaultSecret);

            // when
            SecureData actualSecureData = await this.secureDataService
                .AddOrModifySecureData(inputSecureData);

            // then
            actualSecureData.Should().BeEquivalentTo(expectedSecureData);

            this.keyVaultSecretBrokerMock.Verify(broker =>
                broker.CreateOrUpdateKeyVaultSecretAsync(It.Is(SameKeyVaultSecretAs(inputKeyVaultSecret))),
                    Times.Once);

            this.keyVaultSecretBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}