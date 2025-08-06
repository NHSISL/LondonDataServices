// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(IngressDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnRollbackIfDependencyValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            string someEncryptedFileName = GetRandomString();

            var expectedDependencyException =
                new IngressOrchestrationDependencyValidationException(
                    message:
                        "Ingress orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveAllIngestionTrackingsAsync())
                    .ThrowsAsync(dependancyValidationException);

            //when
            ValueTask rollbackIngestionTrackingItemTask =
                this.ingressOrchestrationService.RollbackIngestionTrackingItemAsync(someEncryptedFileName);

            IngressOrchestrationDependencyValidationException actualException =
              await Assert.ThrowsAsync<IngressOrchestrationDependencyValidationException>(
                  rollbackIngestionTrackingItemTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(IngressDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRollbackIfDependencyExceptionOccursAndLogItAsync(
         Xeption dependancyException)
        {
            // given
            string someEncryptedFileName = GetRandomString();

            var expectedDependencyException =
                new IngressOrchestrationDependencyException(
                    message: "Ingress orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveAllIngestionTrackingsAsync())
                   .ThrowsAsync(dependancyException);

            // when
            ValueTask rollbackIngestionTrackingItemTask =
                this.ingressOrchestrationService.RollbackIngestionTrackingItemAsync(someEncryptedFileName);

            IngressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<IngressOrchestrationDependencyException>(
                    rollbackIngestionTrackingItemTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRollbackIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someEncryptedFileName = GetRandomString();
            var serviceException = new Exception();

            var failedIngressOrchestrationServiceException =
                new FailedIngressOrchestrationServiceException(
                    message: "Failed ingress orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngressOrchestrationServiceException =
                new IngressOrchestrationServiceException(
                    message: "Ingress orchestration service error occurred, please contact support.",
                    innerException: failedIngressOrchestrationServiceException);

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveAllIngestionTrackingsAsync())
                  .ThrowsAsync(serviceException);

            // when
            ValueTask rollbackIngestionTrackingItemTask =
                this.ingressOrchestrationService.RollbackIngestionTrackingItemAsync(someEncryptedFileName);

            IngressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<IngressOrchestrationServiceException>(
                    rollbackIngestionTrackingItemTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedIngressOrchestrationServiceException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngressOrchestrationServiceException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
