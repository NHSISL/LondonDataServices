// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DecryptionDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnDecryptIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            string randomFileName = GetRandomMessage();

            var expectedDependencyException =
                new DecryptionOrchestrationDependencyValidationException(
                    message: "Decryption orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
               service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName))
                   .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<(string, Guid)> decryptTask = this.decryptionOrchestrationService.DecryptAsync(
                randomFileName,
                inputSubscriberCredential);

            DecryptionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationDependencyValidationException>(decryptTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingServiceMock.Verify(service =>
             service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.cryptographyServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.downloadServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DecryptionDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnDycryptIfDependencyExceptionOccursAndLogItAsync(
            Xeption dependancyException)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            string randomFileName = GetRandomMessage();

            var expectedDependencyException =
                new DecryptionOrchestrationDependencyException(
                    message: "Decryption orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
              service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName))
                  .ThrowsAsync(dependancyException);

            // when
            ValueTask<(string, Guid)> decryptTask = this.decryptionOrchestrationService.DecryptAsync(
                randomFileName,
                inputSubscriberCredential);

            DecryptionOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationDependencyException>(decryptTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingServiceMock.Verify(service =>
             service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.downloadServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            string randomFileName = GetRandomMessage();
            var serviceException = new Exception();

            var failedDecryptionOrchestrationServiceException =
                new FailedDecryptionOrchestrationServiceException(
                    message: "Failed Decryption orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDecryptionOrchestrationServiceException =
                new DecryptionOrchestrationServiceException(
                    message: "Decryption orchestration service error occurred, please contact support.",
                    innerException: failedDecryptionOrchestrationServiceException);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<(string, Guid)> processTask = this.decryptionOrchestrationService.DecryptAsync(
                randomFileName,
                inputSubscriberCredential);

            DecryptionOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDecryptionOrchestrationServiceException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecryptionOrchestrationServiceException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.downloadServiceMock.VerifyNoOtherCalls();
        }
    }
}