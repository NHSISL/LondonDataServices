// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Orchestrations.Decryptions.Exceptions;
using Moq;
using Xeptions;

namespace LHDS.Landings.Client.Tests.Unit.Services.Orchestrations.Decryptions
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
                    dependancyValidationException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
               service.RetrieveIngestionTrackingByIdAsync(randomFileName))
                   .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask decryptTask = this.decryptionOrchestrationService.DecryptAsync(randomFileName);

            DecryptionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationDependencyValidationException>(decryptTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingServiceMock.Verify(service =>
             service.RetrieveIngestionTrackingByIdAsync(randomFileName),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.decryptionServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
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
                    dependancyException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
              service.RetrieveIngestionTrackingByIdAsync(randomFileName))
                  .ThrowsAsync(dependancyException);

            // when
            ValueTask decryptTask = this.decryptionOrchestrationService.DecryptAsync(randomFileName);

            DecryptionOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationDependencyException>(decryptTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingServiceMock.Verify(service =>
             service.RetrieveIngestionTrackingByIdAsync(randomFileName),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            string randomFileName = GetRandomMessage();
            var serviceException = new Exception();

            var failedDecryptionOrchestrationServiceException =
                new FailedDecryptionOrchestrationServiceException(serviceException);

            var expectedDecryptionOrchestrationServiceException =
                new DecryptionOrchestrationServiceException(failedDecryptionOrchestrationServiceException);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(randomFileName))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask processTask = this.decryptionOrchestrationService.DecryptAsync(randomFileName);

            DecryptionOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDecryptionOrchestrationServiceException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(randomFileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionOrchestrationServiceException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}