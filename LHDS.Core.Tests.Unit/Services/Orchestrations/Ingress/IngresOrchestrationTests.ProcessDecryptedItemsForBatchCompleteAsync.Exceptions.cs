// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using LHDS.Core.Services.Orchestrations.Ingress;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionForBatchCompleteExceptionOnProcessDecryptedItemsForBatchCompleteAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            randomIngestionTracking.LastBatchCompleteCheck = randomDateTime.AddMinutes(-15);
            randomIngestionTracking.Decrypted = true;
            randomIngestionTracking.IsDownloaded = true;
            randomIngestionTracking.IsBatchComplete = false;

            List<IngestionTracking> storageIngestionTrackings = new List<IngestionTracking>
            {
                randomIngestionTracking
            };

            IngestionTracking failedToUpdateIngestionTracking = randomIngestionTracking.DeepClone();
            failedToUpdateIngestionTracking.LastBatchCompleteCheck = randomDateTime.AddMinutes(15);

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

            this.ingestionTrackingProcessingServiceMock
                .SetupSequence(service => service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(storageIngestionTrackings.AsQueryable())
                        .ReturnsAsync(new List<IngestionTracking>().AsQueryable());

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(randomIngestionTracking.Id))
                    .ReturnsAsync(randomIngestionTracking);

            Exception someException = new Exception(message: GetRandomString());

            this.dateTimeBrokerMock.Setup(service =>
                service.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTime);

            ingressOrchestrationServiceMock.Setup(service =>
                service.CheckForBatchCompleteAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(someException);

            var aggregateException = new AggregateException(
                "One or more errors occurred while checking for batch completion.",
                someException);

            var batchCompleteException = new BatchCompleteException(
                message: "One or more errors occurred while checking for batch completion.",
                innerException: aggregateException);

            var expectedDependencyException =
                new IngressOrchestrationDependencyException(
                    message: "Ingress orchestration dependency error occurred, fix the errors and try again.",
                    innerException: batchCompleteException.InnerException as Xeption);

            // when
            ValueTask checkForBatchCompleteTask =
                ingressOrchestrationServiceMock.Object.ProcessDecryptedItemsForBatchCompleteAsync();

            IngressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<IngressOrchestrationDependencyException>(checkForBatchCompleteTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.dateTimeBrokerMock.Verify(service =>
                service.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.ingestionTrackingProcessingServiceMock
                .Verify(service => service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Exactly(2));

            ingressOrchestrationServiceMock.Verify(service =>
                service.CheckForBatchCompleteAsync(storageIngestionTrackings.First().Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(randomIngestionTracking.Id),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(failedToUpdateIngestionTracking))),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(IngressDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnProcessDecryptedItemsForBatchCompleteIfDependencyValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            // given
            var expectedDependencyException =
                new IngressOrchestrationDependencyValidationException(
                    message:
                        "Ingress orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dependancyValidationException);

            //when
            ValueTask checkForBatchCompleteTask =
                this.ingressOrchestrationService.ProcessDecryptedItemsForBatchCompleteAsync();

            IngressOrchestrationDependencyValidationException actualException =
              await Assert.ThrowsAsync<IngressOrchestrationDependencyValidationException>(
                  checkForBatchCompleteTask.AsTask);

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

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(IngressDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessDecryptedItemsForBatchCompleteIfDependencyExceptionOccursAndLogItAsync(
         Xeption dependancyException)
        {
            // given
            var expectedDependencyException =
                new IngressOrchestrationDependencyException(
                    message: "Ingress orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                   .ThrowsAsync(dependancyException);

            // when
            ValueTask checkForBatchCompleteTask =
                this.ingressOrchestrationService.ProcessDecryptedItemsForBatchCompleteAsync();

            IngressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<IngressOrchestrationDependencyException>(checkForBatchCompleteTask.AsTask);

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

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessDecryptedItemsForBatchCompleteIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngressOrchestrationServiceException =
                new FailedIngressOrchestrationServiceException(
                    message: "Failed ingress orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngressOrchestrationServiceException =
                new IngressOrchestrationServiceException(
                    message: "Ingress orchestration service error occurred, please contact support.",
                    innerException: failedIngressOrchestrationServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask checkForBatchCompleteTask =
                this.ingressOrchestrationService.ProcessDecryptedItemsForBatchCompleteAsync();

            IngressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<IngressOrchestrationServiceException>(checkForBatchCompleteTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedIngressOrchestrationServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
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
