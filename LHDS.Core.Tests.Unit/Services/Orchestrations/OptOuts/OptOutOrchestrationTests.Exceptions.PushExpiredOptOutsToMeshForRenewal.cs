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
        public async Task ShouldThrowDependencyValidationOnPushIfDependValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            // Given
            var expectedDependencyException =
                new OptOutOrchestrationDependencyValidationException(
                    message: "Opt Out orchestration dependency validation errors occurred, " +
                        "fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.optOutProcessingServiceMock.Setup(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(It.IsAny<int>()))
                    .ThrowsAsync(dependancyValidationException);

            // When
            ValueTask<MeshMessage> pushExpiredOptOutsToMeshIfExpiredTask =
                this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            OptOutOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationDependencyValidationException>(
                    pushExpiredOptOutsToMeshIfExpiredTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.optOutProcessingServiceMock.Verify(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
        public async Task ShouldThrowDependencyExceptionOnPushIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // Given
            var expectedDependencyException =
                new OptOutOrchestrationDependencyException(
                    message: "Opt Out orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.optOutProcessingServiceMock.Setup(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(It.IsAny<int>()))
                    .ThrowsAsync(dependancyException);

            // When
            ValueTask<MeshMessage> pushExpiredOptOutsToMeshIfExpiredTask =
                this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            OptOutOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationDependencyException>(
                    pushExpiredOptOutsToMeshIfExpiredTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.optOutProcessingServiceMock.Verify(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
        public async Task ShouldThrowServiceExceptionOnPushIfServiceErrorOccursAndLogItAsync()
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
                    innerException: failedOptOutOrchestrationServiceException);

            this.optOutProcessingServiceMock.Setup(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(It.IsAny<int>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<MeshMessage> pushExpiredOptOutsToMeshIfExpiredTask =
                this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            OptOutOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(
                    pushExpiredOptOutsToMeshIfExpiredTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedOptOrchestrationServiceException);

            this.optOutProcessingServiceMock.Verify(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
