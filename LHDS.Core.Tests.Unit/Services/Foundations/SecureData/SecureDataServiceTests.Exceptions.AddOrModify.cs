// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SecureDatas
{
    public partial class SecureDataServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddOrModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            dynamic randomSecret = CreateRandomSecret();
            SecureData inputSecureData = CreateSecureDataFromRandomSecret(randomSecret);
            var serviceException = new Exception();

            var failedSecureDataServiceException =
                new FailedSecureDataServiceException(
                    message: "Failed secure data service occurred, please contact support",
                    innerException: serviceException);

            var expectedSecureDataServiceException =
                new SecureDataServiceException(
                    message: "Secure data service error occurred, contact support.",
                    innerException: failedSecureDataServiceException);

            this.secureDataBrokerMock.Setup(broker =>
                broker.CreateOrUpdateKeyVaultSecretAsync(It.IsAny<KeyVaultSecret>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SecureData> addSecureDataTask =
                this.secureDataService.AddOrModifySecureData(inputSecureData);

            SecureDataServiceException actualSecureDataServiceException =
                await Assert.ThrowsAsync<SecureDataServiceException>(
                    addSecureDataTask.AsTask);

            // then
            actualSecureDataServiceException.Should()
                .BeEquivalentTo(expectedSecureDataServiceException);

            this.secureDataBrokerMock.Verify(broker =>
                broker.CreateOrUpdateKeyVaultSecretAsync(It.IsAny<KeyVaultSecret>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSecureDataServiceException))),
                        Times.Once);

            this.secureDataBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}