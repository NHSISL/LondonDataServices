// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public async Task ShouldThrowAggregateDependencyValidationExceptionOnProcessIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            List<string> keyTypes = new List<string>
            {
                "FtpPassPhrase",
                "FtpPrivateKey",
                "GpgPassPhrase",
                "GpgPrivateKey",
            };

            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();
            List<Exception> exceptions = new List<Exception>();

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            foreach (string keyType in keyTypes)
            {
                this.secureDataServiceMock.Setup(service =>
                    service.AddOrModifySecureData(It.IsAny<SecureData>()))
                        .ThrowsAsync(dependencyValidationException);

                var subscriberCredentialProcessingDependencyValidationException =
                    new SubscriberCredentialProcessingDependencyValidationException(
                        message: "Subscriber credential processing dependency validation error occurred, " +
                        "please try again.",
                     innerException: dependencyValidationException.InnerException as Xeption);

                exceptions.Add(subscriberCredentialProcessingDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to add or modify for {exceptions.Count} secure data",
                    exceptions);

            var failedSubscriberCredentialProcessingServiceException =
                new FailedSubscriberCredentialProcessingServiceException(
                    message: "Failed subscriber credential aggregate processing service error occurred, " +
                    "contact support.",
                    innerException: aggregateException);

            var expectedSubscriberCredentialProcessingServiceException =
                new SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, contact support.",
                    innerException: failedSubscriberCredentialProcessingServiceException);

            // when
            ValueTask<SubscriberCredential> secureDataAddTask =
                this.secureDataProcessingService.AddOrModifySecureDataAsync(inputSubscriberCredential);

            SubscriberCredentialProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialProcessingDependencyValidationException>(
                    secureDataAddTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialProcessingServiceException);

            foreach (var keyType in keyTypes)
            {
                this.secureDataServiceMock.Verify(service =>
                    service.AddOrModifySecureData(It.IsAny<SecureData>()),
                        Times.Once);
            }

            var secureDataProcessingDependencyValidationException =
                new SubscriberCredentialProcessingDependencyValidationException(
                    message: "Subscriber credential processing dependency validation error occurred, " +
                    "please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     secureDataProcessingDependencyValidationException))),
                         Times.Exactly(keyTypes.Count));

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedSubscriberCredentialProcessingServiceException))),
                         Times.Once);

            this.secureDataServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowAggregateDependencyExceptionOnProcessIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            List<string> keyTypes = new List<string>
            {
                "FtpPassPhrase",
                "FtpPrivateKey",
                "GpgPassPhrase",
                "GpgPrivateKey",
            };

            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();
            List<Exception> exceptions = new List<Exception>();

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            foreach (var keyType in keyTypes)
            {
                this.secureDataServiceMock.Setup(service =>
                    service.AddOrModifySecureData(It.IsAny<SecureData>()))
                        .ThrowsAsync(dependencyException);

                var subscriberCredentialProcessingDependencyException =
                    new SubscriberCredentialProcessingDependencyException(
                        message: "Subscriber credential processing dependency validation error occurred, " +
                        "please try again.",
                     innerException: dependencyException.InnerException as Xeption);

                exceptions.Add(subscriberCredentialProcessingDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to add or modify for {exceptions.Count} secure data",
                    exceptions);

            var failedSubscriberCredentialProcessingServiceException =
                new FailedSubscriberCredentialProcessingServiceException(
                    message: "Failed subscriber credential aggregate processing service error occurred, " +
                    "contact support.",
                    innerException: aggregateException);

            var expectedSubscriberCredentialProcessingServiceException =
                new SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, contact support.",
                    innerException: failedSubscriberCredentialProcessingServiceException);

            // when
            ValueTask<SubscriberCredential> secureDataAddTask =
                this.secureDataProcessingService.AddOrModifySecureDataAsync(inputSubscriberCredential);

            SubscriberCredentialProcessingDependencyException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialProcessingDependencyException>(
                    secureDataAddTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialProcessingServiceException);

            foreach (var keyType in keyTypes)
            {
                this.secureDataServiceMock.Verify(service =>
                    service.AddOrModifySecureData(It.IsAny<SecureData>()),
                        Times.Once);
            }

            var secureDataProcessingDependencyException =
                new SubscriberCredentialProcessingDependencyException(
                    message: "Subscriber credential processing dependency error occurred, " +
                    "please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     secureDataProcessingDependencyException))),
                         Times.Exactly(keyTypes.Count));

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedSubscriberCredentialProcessingServiceException))),
                         Times.Once);

            this.secureDataServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnProcessIfErrorsAndLogItAsync()
        {
            // Given
            List<string> keyTypes = new List<string>
                {
                    "FtpPassPhrase",
                    "FtpPrivateKey",
                    "GpgPassPhrase",
                    "GpgPrivateKey",
                };

            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();
            var serviceException = new Exception();
            List<Exception> exceptions = new List<Exception>();

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            var innerfailedSubscriberCredentialProcessingServiceException =
                new FailedSubscriberCredentialProcessingServiceException(
                    message: "Failed subscriber credential processing service error occurred, contact support.",
                    innerException: serviceException);

            var innerSubscriberCredentialProcessingServiceException =
                new SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, contact support.",
                    innerException: innerfailedSubscriberCredentialProcessingServiceException);

            foreach (var keyType in keyTypes)
            {
                this.secureDataServiceMock.Setup(service =>
                    service.AddOrModifySecureData(It.IsAny<SecureData>()))
                        .ThrowsAsync(serviceException);

                exceptions.Add(innerSubscriberCredentialProcessingServiceException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to add or modify for {exceptions.Count} secure data",
                    exceptions);

            var failedSubscriberCredentialProcessingServiceException =
                new FailedSubscriberCredentialProcessingServiceException(
                    message: "Failed subscriber credential aggregate processing service error occurred, " +
                    "contact support.",
                    innerException: aggregateException);

            var expectedSubscriberCredentialProcessingServiceException =
                new SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, contact support.",
                    innerException: failedSubscriberCredentialProcessingServiceException);

            // when
            ValueTask<SubscriberCredential> secureDataAddTask =
                this.secureDataProcessingService.AddOrModifySecureDataAsync(inputSubscriberCredential);

            SubscriberCredentialProcessingServiceException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialProcessingServiceException>(
                    secureDataAddTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialProcessingServiceException);

            foreach (var keyType in keyTypes)
            {
                this.secureDataServiceMock.Verify(service =>
                    service.AddOrModifySecureData(It.IsAny<SecureData>()),
                        Times.Once);
            }

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     innerSubscriberCredentialProcessingServiceException))),
                         Times.Exactly(keyTypes.Count));

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedSubscriberCredentialProcessingServiceException))),
                         Times.Once);

            this.secureDataServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

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