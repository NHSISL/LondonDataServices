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
            Guid supplierId = Guid.NewGuid();
            IngestionTracking randomIngestionTrackingOne = CreateRandomIngestionTracking();
            randomIngestionTrackingOne.Decrypted = true;
            randomIngestionTrackingOne.IsBatchComplete = false;
            randomIngestionTrackingOne.SupplierId = supplierId;

            IngestionTracking randomIngestionTrackingTwo = randomIngestionTrackingOne.DeepClone();
            randomIngestionTrackingTwo.Id = Guid.NewGuid();

            List<IngestionTracking> storageIngestionTrackings = new List<IngestionTracking>
            {
                randomIngestionTrackingOne,
                randomIngestionTrackingTwo
            };

            var ingressOrchestrationServiceMock = new Mock<IngressOrchestrationService>(
                this.ingestionTrackingProcessingServiceMock.Object,
                this.specificationObjectProcessingServiceMock.Object,
                this.documentProcessingServiceMock.Object,
                this.landingConfiguration,
                this.blobContainers,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object)
            {
                CallBase = true
            };

            this.ingestionTrackingProcessingServiceMock
                .SetupSequence(service => service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(storageIngestionTrackings.AsQueryable())
                        .ReturnsAsync(new List<IngestionTracking>().AsQueryable());

            Exception someException = new Exception(message: GetRandomString());

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
                ingressOrchestrationServiceMock.Object.ProcessDecryptedItemsForBatchCompleteAsync(supplierId);

            IngressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<IngressOrchestrationDependencyException>(checkForBatchCompleteTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

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
            Guid supplierId = Guid.NewGuid();

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
            ValueTask checkForBatchCompleteTask =
                this.ingressOrchestrationService.ProcessDecryptedItemsForBatchCompleteAsync(supplierId);

            IngressOrchestrationDependencyValidationException actualException =
              await Assert.ThrowsAsync<IngressOrchestrationDependencyValidationException>(
                  checkForBatchCompleteTask.AsTask);

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
        public async Task ShouldThrowDependencyExceptionOnProcessDecryptedItemsForBatchCompleteIfDependencyExceptionOccursAndLogItAsync(
         Xeption dependancyException)
        {
            // given
            Guid supplierId = Guid.NewGuid();

            var expectedDependencyException =
                new IngressOrchestrationDependencyException(
                    message: "Ingress orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveAllIngestionTrackingsAsync())
                   .ThrowsAsync(dependancyException);

            // when
            ValueTask checkForBatchCompleteTask =
                this.ingressOrchestrationService.ProcessDecryptedItemsForBatchCompleteAsync(supplierId);

            IngressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<IngressOrchestrationDependencyException>(checkForBatchCompleteTask.AsTask);

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
        public async Task ShouldThrowServiceExceptionOnProcessDecryptedItemsForBatchCompleteIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid supplierId = Guid.NewGuid();
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
            ValueTask checkForBatchCompleteTask =
                this.ingressOrchestrationService.ProcessDecryptedItemsForBatchCompleteAsync(supplierId);

            IngressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<IngressOrchestrationServiceException>(checkForBatchCompleteTask.AsTask);

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
