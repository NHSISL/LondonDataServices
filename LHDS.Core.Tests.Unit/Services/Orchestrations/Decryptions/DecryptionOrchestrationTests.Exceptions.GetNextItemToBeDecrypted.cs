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
        public async Task ShouldThrowDependencyValidationOnGetNextItemToBeDecryptedIfErrorOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var expectedDependencyException =
                new DecryptionOrchestrationDependencyValidationException(
                    message: "Decryption orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(service =>
               service.GetCurrentDateTimeOffsetAsync())
                   .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<string> decryptTask = this.decryptionOrchestrationService.GetNextItemToBeDecrypted();

            DecryptionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationDependencyValidationException>(decryptTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.dateTimeBrokerMock.Verify(service =>
                service.GetCurrentDateTimeOffsetAsync(),
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
        public async Task ShouldThrowDependencyExceptionOnGetNextItemToBeDecryptedIfErrorOccursAndLogItAsync(
            Xeption dependancyException)
        {
            // given
            var expectedDependencyException =
                new DecryptionOrchestrationDependencyException(
                    message: "Decryption orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(service =>
               service.GetCurrentDateTimeOffsetAsync())
                  .ThrowsAsync(dependancyException);

            // when
            ValueTask<string> decryptTask = this.decryptionOrchestrationService.GetNextItemToBeDecrypted();

            DecryptionOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationDependencyException>(decryptTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.dateTimeBrokerMock.Verify(service =>
                service.GetCurrentDateTimeOffsetAsync(),
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
        public async Task ShouldThrowServiceExceptionOnGetNextItemToBeDecryptedIfErrorOccursAndLogItAsync()
        {
            //Given
            var serviceException = new Exception();

            var failedDecryptionOrchestrationServiceException =
                new FailedDecryptionOrchestrationServiceException(
                    message: "Failed Decryption orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDecryptionOrchestrationServiceException =
                new DecryptionOrchestrationServiceException(
                    message: "Decryption orchestration service error occurred, please contact support.",
                    innerException: failedDecryptionOrchestrationServiceException);

            this.dateTimeBrokerMock.Setup(service =>
               service.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> decryptTask = this.decryptionOrchestrationService.GetNextItemToBeDecrypted();

            DecryptionOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationServiceException>(decryptTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDecryptionOrchestrationServiceException);

            this.dateTimeBrokerMock.Verify(service =>
                service.GetCurrentDateTimeOffsetAsync(),
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