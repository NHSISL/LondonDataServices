// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnCheckForBatchCompleteIfArgumentsIsNullAndLogItAsync()
        {
            // given
            Guid nullIngestionId = Guid.Empty;

            var invalidArgumentIngresOrchestrationException =
                new InvalidArgumentIngressOrchestrationException(
                    message: "Invalid ingress orchestration argument(s), " +
                    "please correct the errors and try again.");

            invalidArgumentIngresOrchestrationException.AddData(
                key: "ingestionTrackingId",
                values: "Id is required");

            var expectedIngresOrchestrationValidationException =
                new IngressOrchestrationValidationException(
                    message: "Ingress orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentIngresOrchestrationException);

            // when
            ValueTask batchCompleteTask = this.ingressOrchestrationService
                .CheckForBatchCompleteAsync(ingestionTrackingId: nullIngestionId);

            IngressOrchestrationValidationException actualIngressOrchestrationValidationException =
                await Assert.ThrowsAsync<IngressOrchestrationValidationException>(batchCompleteTask.AsTask);

            // then
            actualIngressOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedIngresOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngresOrchestrationValidationException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnCheckForBatchCompleteIfStorageIsNullAndLogItAsync()
        {
            // given
            Guid randomIngestionId = Guid.NewGuid();
            Guid inputIngestionTracking = randomIngestionId;

            var notFoundIngressOrchestrationException =
            new NotFoundIngressOrchestrationException(
                    message: $"Couldn't find ingestion tracking with Id: {inputIngestionTracking}.");

            var expectedIngresOrchestrationValidationException =
                new IngressOrchestrationValidationException(
                    message: "Ingress orchestration validation errors occurred, please try again.",
                    innerException: notFoundIngressOrchestrationException);

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking))
                    .ThrowsAsync(notFoundIngressOrchestrationException);

            // when
            ValueTask batchCompleteTask = this.ingressOrchestrationService
                .CheckForBatchCompleteAsync(ingestionTrackingId: inputIngestionTracking);

            IngressOrchestrationValidationException actualIngressOrchestrationValidationException =
                await Assert.ThrowsAsync<IngressOrchestrationValidationException>(batchCompleteTask.AsTask);

            // then
            actualIngressOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedIngresOrchestrationValidationException);

            this.ingestionTrackingProcessingServiceMock
                .Verify(service => service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngresOrchestrationValidationException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnCheckForBatchCompleteIfConfigIsNullAndLogItAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking storageIngestionTracking = randomIngestionTracking.DeepClone();

            var noConfigIngressOrchestrationException = new NoConfigIngressOrchestrationException(
                message:
                    "No specification object files found for dataset specification id: " +
                    $"{storageIngestionTracking.DataSetSpecificationId}");

            var expectedIngresOrchestrationValidationException =
                new IngressOrchestrationValidationException(
                    message: "Ingress orchestration validation errors occurred, please try again.",
                    innerException: noConfigIngressOrchestrationException);

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveIngestionTrackingByIdAsync(randomIngestionTracking.Id))
                .ReturnsAsync(storageIngestionTracking);

            // when
            ValueTask batchCompleteTask = this.ingressOrchestrationService
                .CheckForBatchCompleteAsync(ingestionTrackingId: randomIngestionTracking.Id);

            IngressOrchestrationValidationException actualIngressOrchestrationValidationException =
                await Assert.ThrowsAsync<IngressOrchestrationValidationException>(batchCompleteTask.AsTask);

            // then
            actualIngressOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedIngresOrchestrationValidationException);

            this.ingestionTrackingProcessingServiceMock
                .Verify(service => service.RetrieveIngestionTrackingByIdAsync(randomIngestionTracking.Id),
                    Times.Once);

            this.specificationObjectProcessingServiceMock.Verify(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationId(
                    storageIngestionTracking.DataSetSpecificationId),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngresOrchestrationValidationException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
