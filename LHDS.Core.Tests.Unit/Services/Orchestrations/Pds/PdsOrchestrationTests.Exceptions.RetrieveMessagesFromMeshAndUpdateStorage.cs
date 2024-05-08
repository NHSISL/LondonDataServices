// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public async Task ShouldThrowDependencyValidationOnRetrieveAndUpdateIfDependencyValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            var expectedDependencyException =
                new PdsOrchestrationDependencyValidationException(
                    message: "PDS orchestration validation errors occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
              service.RetrieveMessageIdsFromInboxAsync())
                .Throws(dependancyValidationException);

            //when
            ValueTask<List<PdsAudit>> actualPdsAudits =
                this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            PdsOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationDependencyValidationException>(
                    actualPdsAudits.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshServiceMock.Verify(broker =>
                broker.RetrieveMessageIdsFromInboxAsync(),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(PdsDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveAndUpdateIfDependencyExceptionOccursAndLogItAsync(
         Xeption dependancyException)
        {
            // given
            var expectedDependencyException =
                new PdsOrchestrationDependencyException(
                    message: "PDS orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageIdsFromInboxAsync())
                    .Throws(dependancyException);

            // when
            ValueTask<List<PdsAudit>> retrievePdsAudits =
                this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            PdsOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationDependencyException>(retrievePdsAudits.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAndUpdateIfServiceErrorOccursAndLogItAsync()
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
                service.RetrieveMessageIdsFromInboxAsync())
                    .Throws(serviceException);

            // when
            ValueTask<List<PdsAudit>> retrievePdsAudits =
                this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            PdsOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationServiceException>(retrievePdsAudits.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedPdsOrchestrationServiceException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsOrchestrationServiceException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
