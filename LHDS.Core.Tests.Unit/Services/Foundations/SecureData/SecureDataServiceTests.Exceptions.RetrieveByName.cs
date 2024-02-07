// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure;
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveIfSecureDataErrorOccursAndLogItAsync()
        {
            // given
            string someSecretName = GetRandomString();

            var requestFailedException =
                new RequestFailedException(message: "Failed secure data request error occurred, contact support.");

            var failedSecureDataException =
                new FailedSecureDataException(
                    message: "Failed secure data error occurred, contact support.",
                    innerException: requestFailedException);

            var expectedSecureDataDependencyException =
                new SecureDataDependencyException(
                    message: "Secure data dependency errors occurred, contact support.",
                    innerException: failedSecureDataException);

            this.secureDataBrokerMock.Setup(broker =>
                broker.GetKeyVaultSecretAsync(someSecretName))
                    .ThrowsAsync(requestFailedException);

            // when
            ValueTask<SecureData> retrieveSecureDataTask =
                this.secureDataService.RetrieveSecretDataByNameAsync(someSecretName);

            SecureDataDependencyException actualSecureDataDependencyException =
                await Assert.ThrowsAsync<SecureDataDependencyException>(
                    retrieveSecureDataTask.AsTask);

            // then
            actualSecureDataDependencyException.Should()
                .BeEquivalentTo(expectedSecureDataDependencyException);

            this.secureDataBrokerMock.Verify(broker =>
                broker.GetKeyVaultSecretAsync(someSecretName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSecureDataDependencyException))),
                       Times.Once);

            this.secureDataBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someSecretName = GetRandomString();
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
                broker.GetKeyVaultSecretAsync(someSecretName))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SecureData> retrieveSecureDataTask =
                this.secureDataService.RetrieveSecretDataByNameAsync(someSecretName);

            SecureDataServiceException actualSecureDataServiceException =
                await Assert.ThrowsAsync<SecureDataServiceException>(
                    retrieveSecureDataTask.AsTask);

            // then
            actualSecureDataServiceException.Should()
                .BeEquivalentTo(expectedSecureDataServiceException);

            this.secureDataBrokerMock.Verify(broker =>
                broker.GetKeyVaultSecretAsync(someSecretName),
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