// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SecureDatas
{
    public partial class SecureDataServiceTests
    {
        [Fact]
        public async Task ShouldRemoveSecureDataBySecretNameAsync()
        {
            // given
            string secretName = GetRandomString();

            // when
            await this.secureDataService.RemoveSecureDataAsync(secretName);

            // then
            this.keyVaultSecretBrokerMock.Verify(broker =>
                broker.DeleteKeyVaultSecretAsync(secretName),
                    Times.Once);

            this.keyVaultSecretBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}