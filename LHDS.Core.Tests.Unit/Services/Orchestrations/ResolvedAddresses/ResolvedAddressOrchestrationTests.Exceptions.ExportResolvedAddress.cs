// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Moq;
using Valid8R;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
           ShouldThrowDependencyValidationExceptionOnExportResolvedAddressIfDependencyValidationErrorOccursAndLogItAsync(
           Xeption dependencyValidationException)
        {
            // given
            var expectedResolvedAddressOrchestrationDependencyValidationException =
                new ResolvedAddressOrchestrationDependencyValidationException(
                    message: "Resolved address orchestration dependency validation errors occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<List<Guid>> action =
                this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyValidationException>(
                    action.AsTask);

            // then
            actualException.Should().
                BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyValidationException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationDependencyValidationException))),
                         Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnnExportResolvedAddressIfDependencyErrorOccursAndLogItAsync(
          Xeption dependencyException)
        {
            // given
            var expectedResolvedAddressOrchestrationDependencyException =
                new ResolvedAddressOrchestrationDependencyException(
                    message: "Resolved address orchestration dependency errors occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(dependencyException);

            // when
            ValueTask<List<Guid>> action =
               this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyException>(
                    action.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationDependencyException))),
                         Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnExportResolvedAddressIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressOrchestrationServiveException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(serviceException);

            // when
            ValueTask<List<Guid>> action =
               this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(action.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationServiveException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationServiveException))),
                         Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
           ShouldThrowAggregateDependencyValidationExceptionOnExportResolvedAddressErrorsInLoopAndLogItAsync(
           Xeption dependencyValidationException)
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 2);
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            List<ResolvedAddress> storageBatchResolvedAddresses = randomResolvedAddresses.DeepClone();
            Guid batchReference = Guid.NewGuid();

            storageBatchResolvedAddresses.ForEach(pra =>
            {
                pra.BatchReference = batchReference;
            });

            List<ResolvedAddress> processingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<ResolvedAddress> failedProcessingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<Exception> exceptions = new List<Exception>();

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
                 service.RetrieveAllResolvedAddresses())
                     .Returns(storageResolvedAddresses.AsQueryable())
                     .Returns(storageBatchResolvedAddresses.AsQueryable())
                     .Returns(new List<ResolvedAddress>().AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(batchReference);

            processingResolvedAddresses.ForEach(pra =>
            {
                pra.IsProcessing = true;
                pra.RetryCount += 1;
                pra.BatchReference = batchReference;
            });

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
               processing.BulkModifyResolvedAddressesAsync(
                   It.Is(SameResolvedAddressListAs(processingResolvedAddresses))))
                    .Throws(dependencyValidationException);

            failedProcessingResolvedAddresses.ForEach(fpra =>
            {
                fpra.IsProcessing = false;
                fpra.IsExported = false;
                fpra.IsProcessed = false;
            });

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
              processing.BulkModifyResolvedAddressesAsync(
                  It.Is(SameResolvedAddressListAs(failedProcessingResolvedAddresses))))
                    .Returns(ValueTask.CompletedTask);

            var innerResolvedAddressOrchestrationDependencyValidationException =
                new ResolvedAddressOrchestrationDependencyValidationException(
                    message: "Resolved address orchestration dependency validation errors occurred, " +
                        "please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            exceptions.Add(innerResolvedAddressOrchestrationDependencyValidationException);

            var aggregateException =
                new AggregateException(
                    $"Unable to export addresses for {exceptions.Count} ResolvedAddresses",
                    exceptions);

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address aggregate orchestration service errors occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            // When
            ValueTask<List<Guid>> action =
                this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationServiceException =
               await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(async () =>
                   await action);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
               .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Exactly(3));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(Valid8.SameObjectAs<List<ResolvedAddress>>(processingResolvedAddresses, output, "First bulk modify:"))),
                        Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(Valid8.SameObjectAs<List<ResolvedAddress>>(storageBatchResolvedAddresses, output, "Second bulk modify:"))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    innerResolvedAddressOrchestrationDependencyValidationException))),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualResolvedAddressOrchestrationServiceException))),
                        Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task
           ShouldThrowAggregateDependencyExceptionOnExportResolvedAddressErrorsInLoopAndLogItAsync(
           Xeption dependencyException)
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 2);
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            List<ResolvedAddress> storageBatchResolvedAddresses = randomResolvedAddresses.DeepClone();
            Guid batchReference = Guid.NewGuid();

            storageBatchResolvedAddresses.ForEach(pra =>
            {
                pra.BatchReference = batchReference;
            });

            List<ResolvedAddress> processingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<ResolvedAddress> failedProcessingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<Exception> exceptions = new List<Exception>();

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
                 service.RetrieveAllResolvedAddresses())
                     .Returns(storageResolvedAddresses.AsQueryable())
                     .Returns(storageBatchResolvedAddresses.AsQueryable())
                     .Returns(new List<ResolvedAddress>().AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(batchReference);

            processingResolvedAddresses.ForEach(pra =>
            {
                pra.IsProcessing = true;
                pra.RetryCount += 1;
                pra.BatchReference = batchReference;
            });

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
               processing.BulkModifyResolvedAddressesAsync(
                   It.Is(SameResolvedAddressListAs(processingResolvedAddresses))))
                    .Throws(dependencyException);

            failedProcessingResolvedAddresses.ForEach(fpra =>
            {
                fpra.IsProcessing = false;
                fpra.IsExported = false;
                fpra.IsProcessed = false;
            });

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
              processing.BulkModifyResolvedAddressesAsync(
                  It.Is(SameResolvedAddressListAs(failedProcessingResolvedAddresses))))
                    .Returns(ValueTask.CompletedTask);

            var innerResolvedAddressOrchestrationDependencyException =
                new ResolvedAddressOrchestrationDependencyException(
                    message: "Resolved address orchestration dependency errors occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            exceptions.Add(innerResolvedAddressOrchestrationDependencyException);

            var aggregateException =
                new AggregateException(
                    $"Unable to export addresses for {exceptions.Count} ResolvedAddresses",
                    exceptions);

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address aggregate orchestration service errors occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            // When
            ValueTask<List<Guid>> action =
                this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationServiceException =
               await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(async () =>
                   await action);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
               .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Exactly(3));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(Valid8.SameObjectAs<List<ResolvedAddress>>(
                        processingResolvedAddresses,
                        output,
                        "First bulk modify:"))),
                        Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(Valid8.SameObjectAs<List<ResolvedAddress>>(
                        storageBatchResolvedAddresses,
                        output,
                        "Second bulk modify:"))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    innerResolvedAddressOrchestrationDependencyException))),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualResolvedAddressOrchestrationServiceException))),
                        Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnExportResolvedAddressErrorsInLoopAndLogItAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 2);
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            List<ResolvedAddress> storageBatchResolvedAddresses = randomResolvedAddresses.DeepClone();
            Guid batchReference = Guid.NewGuid();

            storageBatchResolvedAddresses.ForEach(pra =>
            {
                pra.BatchReference = batchReference;
            });

            List<ResolvedAddress> processingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<ResolvedAddress> failedProcessingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<Exception> exceptions = new List<Exception>();
            var serviceException = new Exception();

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
                 service.RetrieveAllResolvedAddresses())
                     .Returns(storageResolvedAddresses.AsQueryable())
                     .Returns(storageBatchResolvedAddresses.AsQueryable())
                     .Returns(new List<ResolvedAddress>().AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(batchReference);

            processingResolvedAddresses.ForEach(pra =>
            {
                pra.IsProcessing = true;
                pra.RetryCount += 1;
                pra.BatchReference = batchReference;
            });

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
               processing.BulkModifyResolvedAddressesAsync(
                   It.Is(SameResolvedAddressListAs(processingResolvedAddresses))))
                    .Throws(serviceException);

            failedProcessingResolvedAddresses.ForEach(fpra =>
            {
                fpra.IsProcessing = false;
                fpra.IsExported = false;
                fpra.IsProcessed = false;
            });

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
              processing.BulkModifyResolvedAddressesAsync(
                  It.Is(SameResolvedAddressListAs(failedProcessingResolvedAddresses))))
                    .Returns(ValueTask.CompletedTask);

            var innerFailedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var innerResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: innerFailedResolvedAddressOrchestrationServiceException);

            exceptions.Add(innerResolvedAddressOrchestrationServiceException);

            var aggregateException =
                new AggregateException(
                    $"Unable to export addresses for {exceptions.Count} ResolvedAddresses",
                    exceptions);

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address aggregate orchestration service errors occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            // When
            ValueTask<List<Guid>> action =
                this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationServiceException =
               await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(async () =>
                   await action);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
               .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Exactly(3));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(Valid8.SameObjectAs<List<ResolvedAddress>>(
                        processingResolvedAddresses,
                        output,
                        "First bulk modify:"))),
                        Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(Valid8.SameObjectAs<List<ResolvedAddress>>(
                        storageBatchResolvedAddresses,
                        output,
                        "Second bulk modify:"))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    innerResolvedAddressOrchestrationServiceException))),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualResolvedAddressOrchestrationServiceException))),
                        Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
