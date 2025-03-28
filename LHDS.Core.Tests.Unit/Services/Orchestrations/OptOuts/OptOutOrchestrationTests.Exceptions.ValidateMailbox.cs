// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowDependencyValidationOnValidateMailboxIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var expectedDependencyException =
                new OptOutOrchestrationDependencyValidationException(
                    message: "Opt Out orchestration dependency validation errors occurred, " +
                        "fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.meshProcessingServiceMock.Setup(processing =>
                processing.ValidateMailboxAccessAsync())
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<bool> validateMailboxAccessTask =
                this.optOutOrchestrationService.ValidateMailboxAccessAsync();

            OptOutOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationDependencyValidationException>(
                    validateMailboxAccessTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshProcessingServiceMock.Verify(processing =>
                processing.ValidateMailboxAccessAsync(),
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
        public async Task ShouldThrowDependencyExceptionOnValidateMailboxIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            var expectedDependencyException =
                new OptOutOrchestrationDependencyException(
                    message: "Opt Out orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.meshProcessingServiceMock.Setup(processing =>
                processing.ValidateMailboxAccessAsync())
                    .ThrowsAsync(dependancyException);

            // when
            ValueTask<bool> validateMailboxAccessTask =
                this.optOutOrchestrationService.ValidateMailboxAccessAsync();

            OptOutOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationDependencyException>(validateMailboxAccessTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshProcessingServiceMock.Verify(processing =>
                processing.ValidateMailboxAccessAsync(),
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
        public async Task ShouldThrowServiceExceptionOnValidateMailboxIfServiceErrorOccursAndLogItAsync()
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

            this.meshProcessingServiceMock.Setup(processing =>
                processing.ValidateMailboxAccessAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> validateMailboxAccessTask =
                this.optOutOrchestrationService.ValidateMailboxAccessAsync();

            OptOutOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(validateMailboxAccessTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedOptOrchestrationServiceException);

            this.meshProcessingServiceMock.Verify(processing =>
                processing.ValidateMailboxAccessAsync(),
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
