// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SecureDatas
{
    public partial class SecureDataServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveIfSecureDataErrorOccursAndLogItAsync()
        {
            // given
            string someSecretName = GetRandomString();

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

            this.keyVaultSecretBrokerMock.Setup(broker =>
                broker.DeleteKeyVaultSecretAsync(someSecretName))
                    .ThrowsAsync(requestFailedException);

            // when
            ValueTask removeSecureDataTask = this.secureDataService.RemoveSecureDataAsync(someSecretName);

            var actualSecureDataDependencyException =
                 await Assert.ThrowsAsync<SecureDataDependencyException>(removeSecureDataTask.AsTask);

            // then
            actualSecureDataDependencyException.Should()
                .BeEquivalentTo(expectedSecureDataDependencyException);

            this.keyVaultSecretBrokerMock.Verify(broker =>
                broker.DeleteKeyVaultSecretAsync(someSecretName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                    expectedSecureDataDependencyException))),
                        Times.Once);

            this.keyVaultSecretBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someSecretName = GetRandomString();
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
                broker.DeleteKeyVaultSecretAsync(someSecretName))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask removeSecureDataTask = this.secureDataService.RemoveSecureDataAsync(someSecretName);

            var actualSecureDataServiceException =
                 await Assert.ThrowsAsync<SecureDataServiceException>(removeSecureDataTask.AsTask);

            // then
            actualSecureDataServiceException.Should()
                .BeEquivalentTo(expectedSecureDataServiceException);

            this.keyVaultSecretBrokerMock.Verify(broker =>
                broker.DeleteKeyVaultSecretAsync(someSecretName),
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