// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            var expectedSecureDataProcessingDependencyValidationException =
                new SubscriberCredentialProcessingDependencyValidationException(
                    message: "Subscriber credential processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.secureDataServiceMock.Setup(service =>
                service.AddOrModifySecureData(It.IsAny<SecureData>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<SubscriberCredential> secureDataAddTask =
                this.secureDataProcessingService.AddOrModifySecureDataAsync(inputSubscriberCredential);

            SubscriberCredentialProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialProcessingDependencyValidationException>(
                    secureDataAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSecureDataProcessingDependencyValidationException);

            this.secureDataServiceMock.Verify(service =>
                service.AddOrModifySecureData(It.IsAny<SecureData>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedSecureDataProcessingDependencyValidationException))),
                         Times.Once);

            this.secureDataServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            var expectedSubscriberCredentialProcessingDependencyException =
                new SubscriberCredentialProcessingDependencyException(
                    message: "Subscriber credential processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.secureDataServiceMock.Setup(service =>
                service.AddOrModifySecureData(It.IsAny<SecureData>()))
                    .Throws(dependencyException);

            // when
            ValueTask<SubscriberCredential> secureDataAddTask =
                this.secureDataProcessingService.AddOrModifySecureDataAsync(inputSubscriberCredential);

            SubscriberCredentialProcessingDependencyException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialProcessingDependencyException>(secureDataAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberCredentialProcessingDependencyException);

            this.secureDataServiceMock.Verify(service =>
                service.AddOrModifySecureData(It.IsAny<SecureData>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedSubscriberCredentialProcessingDependencyException))),
                         Times.Once);

            this.secureDataServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAsync()
        {
            // given
            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();
            var serviceException = new Exception();

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            var failedSubscriberCredentialProcessingServiceException =
                new FailedSubscriberCredentialProcessingServiceException(
                    message: "Failed subscriber credential processing service error occurred, contact support.",
                    innerException: serviceException);

            var expectedSubscriberCredentialProcessingServiveException =
                new SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, contact support.",
                    innerException: failedSubscriberCredentialProcessingServiceException);

            this.secureDataServiceMock.Setup(service =>
                service.AddOrModifySecureData(It.IsAny<SecureData>()))
                    .Throws(serviceException);

            // when
            ValueTask<SubscriberCredential> addSubscriberCredentialTask =
                this.secureDataProcessingService.AddOrModifySecureDataAsync(inputSubscriberCredential);

            SubscriberCredentialProcessingServiceException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialProcessingServiceException>(
                    addSubscriberCredentialTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberCredentialProcessingServiveException);

            this.secureDataServiceMock.Verify(service =>
                service.AddOrModifySecureData(It.IsAny<SecureData>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedSubscriberCredentialProcessingServiveException))),
                         Times.Once);

            this.secureDataServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}