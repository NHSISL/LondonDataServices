// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(PdsDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnValidateMailboxIfDependencyValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            var expectedDependencyException =
                new PdsOrchestrationDependencyValidationException(
                    message: "PDS orchestration dependency validation errors occurred, fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
              service.ValidateMailboxAccessAsync())
                    .ThrowsAsync(dependancyValidationException);

            //when
            ValueTask<bool> validateMailboxAccessTask =
                this.pdsOrchestrationService.ValidateMailboxAccessAsync(
                    TestContext.Current.CancellationToken);

            PdsOrchestrationDependencyValidationException actualException =
              await Assert.ThrowsAsync<PdsOrchestrationDependencyValidationException>(
                  validateMailboxAccessTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshServiceMock.Verify(service =>
                service.ValidateMailboxAccessAsync(),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(PdsDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnValidateMailboxfDependencyExceptionOccursAndLogItAsync(
         Xeption dependancyException)
        {
            // given
            var expectedDependencyException =
                new PdsOrchestrationDependencyException(
                    message: "PDS orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.ValidateMailboxAccessAsync())
                   .ThrowsAsync(dependancyException);

            // when
            ValueTask<bool> validateMailboxAccessTask =
                this.pdsOrchestrationService.ValidateMailboxAccessAsync(
                    TestContext.Current.CancellationToken);

            PdsOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationDependencyException>(validateMailboxAccessTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshServiceMock.Verify(service =>
                 service.ValidateMailboxAccessAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnValidateMailboxIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedPdsOrchestrationServiceException =
                new FailedPdsOrchestrationServiceException(
                    message: "Failed PDS orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedPdsOrchestrationServiceException =
                new PdsOrchestrationServiceException(
                    message: "PDS orchestration service error occurred, please contact support.",
                    innerException: failedPdsOrchestrationServiceException);

            this.meshServiceMock.Setup(service =>
                service.ValidateMailboxAccessAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> validateMailboxAccessTask =
                this.pdsOrchestrationService.ValidateMailboxAccessAsync(
                    TestContext.Current.CancellationToken);

            PdsOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationServiceException>(validateMailboxAccessTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedPdsOrchestrationServiceException);

            this.meshServiceMock.Verify(service =>
                  service.ValidateMailboxAccessAsync(),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsOrchestrationServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
