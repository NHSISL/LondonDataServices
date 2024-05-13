// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(OptOutDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnUpdateIfDependValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            // Given
            var expectedDependencyException =
                new OptOutOrchestrationDependencyValidationException(
                    message: "Opt Out orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ThrowsAsync(dependancyValidationException);

            // When
            ValueTask<List<MeshMessage>> updateOptOutExpiredOptOutsToMeshIfExpiredTask =
                this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            OptOutOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationDependencyValidationException>(
                    updateOptOutExpiredOptOutsToMeshIfExpiredTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshProcessingServiceMock.Verify(processings =>
                processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OptOutDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnUpdateIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // Given
            var expectedDependencyException =
                new OptOutOrchestrationDependencyException(
                    message: "Opt Out orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ThrowsAsync(dependancyException);

            // When
            ValueTask<List<MeshMessage>> updateOptOutExpiredOptOutsToMeshIfExpiredTask =
                this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            OptOutOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationDependencyException>(
                    updateOptOutExpiredOptOutsToMeshIfExpiredTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshProcessingServiceMock.Verify(processings =>
                processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnUpdateIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedOptOutOrchestrationServiceException =
                new FailedOptOutOrchestrationServiceException(
                    message: "Failed opt out orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedOptOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    failedOptOutOrchestrationServiceException);

            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                   .ThrowsAsync(serviceException);

            // when
            ValueTask<List<MeshMessage>> updateOptOutExpiredOptOutsToMeshIfExpiredTask =
                this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            OptOutOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(
                    updateOptOutExpiredOptOutsToMeshIfExpiredTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedOptOrchestrationServiceException);

            this.meshProcessingServiceMock.Verify(processings =>
                processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOrchestrationServiceException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
