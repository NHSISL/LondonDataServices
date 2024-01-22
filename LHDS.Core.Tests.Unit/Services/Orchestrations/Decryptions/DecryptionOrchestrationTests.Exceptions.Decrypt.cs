// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
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
            string randomFileName = GetRandomMessage();

            var expectedDependencyException =
                new DecryptionOrchestrationDependencyValidationException(
                    message: "Decryption orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
               service.RetrieveIngestionTrackingByFileNameAsync(randomFileName))
                   .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<string> decryptTask = this.decryptionOrchestrationService.DecryptAsync(randomFileName);

            DecryptionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationDependencyValidationException>(decryptTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingServiceMock.Verify(service =>
             service.RetrieveIngestionTrackingByFileNameAsync(randomFileName),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.cryptographyServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DecryptionDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnDycryptIfDependencyExceptionOccursAndLogItAsync(
            Xeption dependancyException)
        {
            // given
            string randomFileName = GetRandomMessage();

            var expectedDependencyException =
                new DecryptionOrchestrationDependencyException(
                    message: "Decryption orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
              service.RetrieveIngestionTrackingByFileNameAsync(randomFileName))
                  .ThrowsAsync(dependancyException);

            // when
            ValueTask<string> decryptTask = this.decryptionOrchestrationService.DecryptAsync(randomFileName);

            DecryptionOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationDependencyException>(decryptTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingServiceMock.Verify(service =>
             service.RetrieveIngestionTrackingByFileNameAsync(randomFileName),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            string randomFileName = GetRandomMessage();
            var serviceException = new Exception();

            var failedDecryptionOrchestrationServiceException =
                new FailedDecryptionOrchestrationServiceException(
                    message: "Failed Decryption orchestration service occurred, please contact support",
                    innerException: serviceException);

            var expectedDecryptionOrchestrationServiceException =
                new DecryptionOrchestrationServiceException(
                    message: "Decryption orchestration service error occurred, contact support.",
                    innerException: failedDecryptionOrchestrationServiceException);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByFileNameAsync(randomFileName))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> processTask = this.decryptionOrchestrationService.DecryptAsync(randomFileName);

            DecryptionOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDecryptionOrchestrationServiceException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByFileNameAsync(randomFileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionOrchestrationServiceException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }
    }
}