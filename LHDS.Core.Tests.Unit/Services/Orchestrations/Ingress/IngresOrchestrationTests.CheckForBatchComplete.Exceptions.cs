// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using LHDS.Core.Services.Orchestrations.Ingress;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(IngressDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnPickupFileAndSendIfDependencyValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            Guid someIngestionTrackingId = Guid.NewGuid();

            var expectedDependencyException =
                new IngressOrchestrationDependencyValidationException(
                    message:
                        "Ingress orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveIngestionTrackingByIdAsync(someIngestionTrackingId))
                    .ThrowsAsync(dependancyValidationException);

            var ingressOrchestrationServiceMock = new Mock<IngressOrchestrationService>(
                this.ingestionTrackingProcessingServiceMock.Object,
                this.specificationObjectProcessingServiceMock.Object,
                this.documentProcessingServiceMock.Object,
                this.landingConfiguration,
                this.blobContainers,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.dateTimeBrokerMock.Object)
            {
                CallBase = true
            };

            //when
            ValueTask checkForBatchCompleteTask =
                ingressOrchestrationServiceMock.Object.CheckForBatchCompleteAsync(someIngestionTrackingId);

            IngressOrchestrationDependencyValidationException actualException =
              await Assert.ThrowsAsync<IngressOrchestrationDependencyValidationException>(
                  checkForBatchCompleteTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(someIngestionTrackingId),
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
        public async Task ShouldThrowDependencyExceptionOnPickupFileAndSendIfDependencyExceptionOccursAndLogItAsync(
         Xeption dependancyException)
        {
            // given
            Guid someIngestionTrackingId = Guid.NewGuid();

            var expectedDependencyException =
                new IngressOrchestrationDependencyException(
                    message: "Ingress orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveIngestionTrackingByIdAsync(someIngestionTrackingId))
                   .ThrowsAsync(dependancyException);

            var ingressOrchestrationServiceMock = new Mock<IngressOrchestrationService>(
                this.ingestionTrackingProcessingServiceMock.Object,
                this.specificationObjectProcessingServiceMock.Object,
                this.documentProcessingServiceMock.Object,
                this.landingConfiguration,
                this.blobContainers,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.dateTimeBrokerMock.Object)
            {
                CallBase = true
            };

            // when
            ValueTask checkForBatchCompleteTask =
                ingressOrchestrationServiceMock.Object.CheckForBatchCompleteAsync(someIngestionTrackingId);

            IngressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<IngressOrchestrationDependencyException>(checkForBatchCompleteTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(someIngestionTrackingId),
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
        public async Task ShouldThrowServiceExceptionOnPickupFileAndSendIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someIngestionTrackingId = Guid.NewGuid();
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
                .Setup(service => service.RetrieveIngestionTrackingByIdAsync(someIngestionTrackingId))
                  .ThrowsAsync(serviceException);

            var ingressOrchestrationServiceMock = new Mock<IngressOrchestrationService>(
                this.ingestionTrackingProcessingServiceMock.Object,
                this.specificationObjectProcessingServiceMock.Object,
                this.documentProcessingServiceMock.Object,
                this.landingConfiguration,
                this.blobContainers,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.dateTimeBrokerMock.Object)
            {
                CallBase = true
            };

            // when
            ValueTask checkForBatchCompleteTask =
                ingressOrchestrationServiceMock.Object.CheckForBatchCompleteAsync(someIngestionTrackingId);

            IngressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<IngressOrchestrationServiceException>(checkForBatchCompleteTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedIngressOrchestrationServiceException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(someIngestionTrackingId),
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
