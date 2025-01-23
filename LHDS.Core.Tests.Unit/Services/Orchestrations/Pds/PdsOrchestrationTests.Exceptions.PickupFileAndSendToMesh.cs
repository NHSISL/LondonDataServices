// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
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
        public async Task ShouldThrowDependencyValidationOnPickupFileAndSendIfDependencyValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            var randomString = GetRandomString();
            var randomBytes = Encoding.UTF8.GetBytes(randomString);
            var randomReceivedFileName = GetRandomString();

            var expectedDependencyException =
                new PdsOrchestrationDependencyValidationException(
                    message: "PDS orchestration dependency validation errors occurred, fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(broker =>
              broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dependancyValidationException);

            //when
            ValueTask<PdsAudit> pickupFileAndSendToMeshTask =
                this.pdsOrchestrationService.PickupFileAndSendToMesh(randomBytes, randomReceivedFileName);

            PdsOrchestrationDependencyValidationException actualException =
              await Assert.ThrowsAsync<PdsOrchestrationDependencyValidationException>(
                  pickupFileAndSendToMeshTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
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
        public async Task ShouldThrowDependencyExceptionOnPickupFileAndSendIfDependencyExceptionOccursAndLogItAsync(
         Xeption dependancyException)
        {
            // given
            var randomString = GetRandomString();
            var randomBytes = Encoding.UTF8.GetBytes(randomString);
            var randomReceivedFileName = GetRandomString();

            var expectedDependencyException =
                new PdsOrchestrationDependencyException(
                    message: "PDS orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTimeOffsetAsync())
                   .ThrowsAsync(dependancyException);

            // when
            ValueTask<PdsAudit> pickupFileAndSendToMeshTask =
              this.pdsOrchestrationService.PickupFileAndSendToMesh(randomBytes, randomReceivedFileName);

            PdsOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationDependencyException>(pickupFileAndSendToMeshTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                 broker.GetCurrentDateTimeOffsetAsync(),
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
        public async Task ShouldThrowServiceExceptionOnPickupFileAndSendIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var randomString = GetRandomString();
            var randomBytes = Encoding.UTF8.GetBytes(randomString);
            var randomReceivedFileName = GetRandomString();
            var serviceException = new Exception();

            var failedPdsOrchestrationServiceException =
                new FailedPdsOrchestrationServiceException(
                    message: "Failed PDS orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedPdsOrchestrationServiceException =
                new PdsOrchestrationServiceException(
                    message: "PDS orchestration service error occurred, please contact support.",
                    innerException: failedPdsOrchestrationServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<PdsAudit> pickupFileAndSendToMeshTask =
                this.pdsOrchestrationService.PickupFileAndSendToMesh(randomBytes, randomReceivedFileName);

            PdsOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationServiceException>(pickupFileAndSendToMeshTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedPdsOrchestrationServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
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
