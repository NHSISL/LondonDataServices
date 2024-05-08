// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure;
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
        public async Task ShouldThrowDependencyExceptionOnAddOrModifyIfSecureDataErrorOccursAndLogItAsync()
        {
            // given
            dynamic randomSecret = CreateRandomSecret();
            SecureData inputSecureData = CreateSecureDataFromRandomSecret(randomSecret);

            var requestFailedException =
                new RequestFailedException(message: "Failed secure data request error occurred, please contact support.");

            var failedSecureDataException =
                new FailedSecureDataException(
                    message: "Failed secure data error occurred, please contact support.",
                    innerException: requestFailedException);

            var expectedSecureDataDependencyException =
                new SecureDataDependencyException(
                    message: "Secure data dependency errors occurred, please contact support.",
                    innerException: failedSecureDataException);

            this.keyVaultSecretBrokerMock.Setup(service =>
                service.CreateOrUpdateKeyVaultSecretAsync(It.IsAny<KeyVaultSecret>()))
                    .ThrowsAsync(requestFailedException);

            // when
            ValueTask<SecureData> addSecureDataTask =
                this.secureDataService.AddOrModifySecureData(inputSecureData);

            SecureDataDependencyException actualException =
                await Assert.ThrowsAsync<SecureDataDependencyException>(
                    addSecureDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedSecureDataDependencyException);

            this.keyVaultSecretBrokerMock.Verify(broker =>
                broker.CreateOrUpdateKeyVaultSecretAsync(It.IsAny<KeyVaultSecret>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSecureDataDependencyException))),
                       Times.Once);

            this.keyVaultSecretBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ExternalDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnSecureDataIfDependencyValidationOccursAndLogItAsync(
            Exception dependencyValidationException)
        {
            // given
            dynamic randomSecret = CreateRandomSecret();
            SecureData inputSecureData = CreateSecureDataFromRandomSecret(randomSecret);

            var failedSecureDataException =
                new FailedSecureDataException(
                    message: "Failed secure data error occurred, please contact support.",
                    innerException: dependencyValidationException);

            var expectedDependencyValidationException =
                new SecureDataDependencyValidationException(
                    message: "Secure data dependency validation errors occurred, fix the errors and try again.",
                    innerException: failedSecureDataException);

            this.keyVaultSecretBrokerMock.Setup(service =>
                service.CreateOrUpdateKeyVaultSecretAsync(It.IsAny<KeyVaultSecret>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<SecureData> addSecureDataTask =
                this.secureDataService.AddOrModifySecureData(inputSecureData);

            SecureDataDependencyValidationException actualException =
                await Assert.ThrowsAsync<SecureDataDependencyValidationException>(
                    addSecureDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyValidationException);

            this.keyVaultSecretBrokerMock.Verify(broker =>
                broker.CreateOrUpdateKeyVaultSecretAsync(It.IsAny<KeyVaultSecret>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyValidationException))),
                       Times.Once);

            this.keyVaultSecretBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddOrModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            dynamic randomSecret = CreateRandomSecret();
            SecureData inputSecureData = CreateSecureDataFromRandomSecret(randomSecret);
            var serviceException = new Exception();

            var failedSecureDataServiceException =
                new FailedSecureDataServiceException(
                    message: "Failed secure data service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSecureDataServiceException =
                new SecureDataServiceException(
                    message: "Secure data service error occurred, please contact support.",
                    innerException: failedSecureDataServiceException);

            this.keyVaultSecretBrokerMock.Setup(broker =>
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

            this.keyVaultSecretBrokerMock.Verify(broker =>
                broker.CreateOrUpdateKeyVaultSecretAsync(It.IsAny<KeyVaultSecret>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSecureDataServiceException))),
                        Times.Once);

            this.keyVaultSecretBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}