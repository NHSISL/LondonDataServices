// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowAggregateDependencyValidationExceptionOnRetrieveIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            List<string> keyTypes = new List<string>
            {
                "FtpPassword",
                "FtpPassPhrase",
                "FtpPrivateKey",
                "GpgPassPhrase",
                "GpgPrivateKey"
            };

            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();
            List<Exception> exceptions = new List<Exception>();

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            foreach (string keyType in keyTypes)
            {
                this.secureDataServiceMock.Setup(service =>
                    service.RetrieveSecretDataByNameAsync(It.IsAny<string>()))
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
                    $"Unable to retrieve {exceptions.Count} secure data item(s)",
                    exceptions);

            var failedSubscriberCredentialProcessingServiceException =
                new FailedSubscriberCredentialProcessingServiceException(
                    message: "Failed subscriber credential aggregate processing service error occurred, " +
                    "contact support.",
                    innerException: aggregateException);

            var expectedSubscriberCredentialProcessingServiceException =
                new SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, please contact support.",
                    innerException: failedSubscriberCredentialProcessingServiceException);

            // when
            ValueTask<SubscriberCredential> secureDataRetrieveTask =
                this.secureDataProcessingService.RetrieveSecretsByKeyVaultKeyNameAsync(inputSubscriberCredential);

            SubscriberCredentialProcessingServiceException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialProcessingServiceException>(
                    secureDataRetrieveTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialProcessingServiceException);

            this.secureDataServiceMock.Verify(service =>
                service.RetrieveSecretDataByNameAsync(It.IsAny<string>()),
                    Times.Exactly(keyTypes.Count));

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
        public async Task ShouldThrowAggregateDependencyExceptionOnRetrieveIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            List<string> keyTypes = new List<string>
            {
                "FtpPassword",
                "FtpPassPhrase",
                "FtpPrivateKey",
                "GpgPassPhrase",
                "GpgPrivateKey"
            };

            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();
            List<Exception> exceptions = new List<Exception>();

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            foreach (var keyType in keyTypes)
            {
                this.secureDataServiceMock.Setup(service =>
                    service.RetrieveSecretDataByNameAsync(It.IsAny<string>()))
                        .ThrowsAsync(dependencyException);

                var subscriberCredentialProcessingDependencyException =
                    new SubscriberCredentialProcessingDependencyException(
                        message: "Subscriber credential processing dependency error occurred, " +
                            "please try again.",
                     innerException: dependencyException.InnerException as Xeption);

                exceptions.Add(subscriberCredentialProcessingDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve {exceptions.Count} secure data item(s)",
                    exceptions);

            var failedSubscriberCredentialProcessingServiceException =
                new FailedSubscriberCredentialProcessingServiceException(
                    message: "Failed subscriber credential aggregate processing service error occurred, " +
                        "contact support.",
                    innerException: aggregateException);

            var expectedSubscriberCredentialProcessingServiceException =
                new SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, please contact support.",
                    innerException: failedSubscriberCredentialProcessingServiceException);

            // when
            ValueTask<SubscriberCredential> secureDataAddTask =
                this.secureDataProcessingService.RetrieveSecretsByKeyVaultKeyNameAsync(inputSubscriberCredential);

            SubscriberCredentialProcessingServiceException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialProcessingServiceException>(
                    secureDataAddTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialProcessingServiceException);

            this.secureDataServiceMock.Verify(service =>
                service.RetrieveSecretDataByNameAsync(It.IsAny<string>()),
                    Times.Exactly(keyTypes.Count));

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
        public async Task ShouldThrowAggregateServiceExceptionOnRetrieveIfErrorsInLoopAndLogItAsync()
        {
            // Given
            List<string> keyTypes = new List<string>
                {
                    "FtpPassword",
                    "FtpPassPhrase",
                    "FtpPrivateKey",
                    "GpgPassPhrase",
                    "GpgPrivateKey"
                };

            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();
            var serviceException = new Exception();
            List<Exception> exceptions = new List<Exception>();

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            var innerfailedSubscriberCredentialProcessingServiceException =
                new FailedSubscriberCredentialProcessingServiceException(
                    message: "Failed subscriber credential processing service error occurred, please contact support.",
                    innerException: serviceException);

            var innerSubscriberCredentialProcessingServiceException =
                new SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, please contact support.",
                    innerException: innerfailedSubscriberCredentialProcessingServiceException);

            foreach (var keyType in keyTypes)
            {
                this.secureDataServiceMock.Setup(service =>
                    service.RetrieveSecretDataByNameAsync(It.IsAny<string>()))
                        .ThrowsAsync(serviceException);

                exceptions.Add(innerSubscriberCredentialProcessingServiceException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve {exceptions.Count} secure data item(s)",
                    exceptions);

            var failedSubscriberCredentialProcessingServiceException =
                new FailedSubscriberCredentialProcessingServiceException(
                    message: "Failed subscriber credential aggregate processing service error occurred, " +
                    "contact support.",
                    innerException: aggregateException);

            var expectedSubscriberCredentialProcessingServiceException =
                new SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, please contact support.",
                    innerException: failedSubscriberCredentialProcessingServiceException);

            // when
            ValueTask<SubscriberCredential> secureDataAddTask =
               this.secureDataProcessingService.RetrieveSecretsByKeyVaultKeyNameAsync(inputSubscriberCredential);

            SubscriberCredentialProcessingServiceException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialProcessingServiceException>(
                    secureDataAddTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialProcessingServiceException);

            this.secureDataServiceMock.Verify(service =>
                service.RetrieveSecretDataByNameAsync(It.IsAny<string>()),
                    Times.Exactly(keyTypes.Count));

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
    }
}
