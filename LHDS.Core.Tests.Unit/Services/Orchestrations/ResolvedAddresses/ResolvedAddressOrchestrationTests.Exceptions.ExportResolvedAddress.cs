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
                service.RetrieveAllResolvedAddressesAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Guid>> exportResolvedAddressesTask =
                this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyValidationException>(
                    exportResolvedAddressesTask.AsTask);

            // then
            actualException.Should().
                BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyValidationException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
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
                service.RetrieveAllResolvedAddressesAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<Guid>> exportResolvedAddressesTask =
               this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyException>(
                    exportResolvedAddressesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
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
                service.RetrieveAllResolvedAddressesAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Guid>> exportResolvedAddressesTask =
                this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(
                    exportResolvedAddressesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationServiveException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
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
            storageResolvedAddresses.ForEach(address => address.IsProcessed = true);
            List<ResolvedAddress> storageBatchResolvedAddresses = randomResolvedAddresses.DeepClone();
            Guid identifier = Guid.NewGuid();
            string exportingAuditMessage = $"Exporting resolved addresses with correlation id {identifier}";
            string exportedAuditMessage = $"Exported resolved addresses with correlation id {identifier}";
            string exportingAuditTitle = "Exporting Resolved Addresses";
            string exportedAuditTitle = "Exported Resolved Addresses";
            string auditType = "Resolved Address Export";

            storageBatchResolvedAddresses.ForEach(pra =>
            {
                pra.BatchReference = identifier;
            });

            List<ResolvedAddress> processingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<ResolvedAddress> failedProcessingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<Exception> exceptions = new List<Exception>();

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
                 service.RetrieveAllResolvedAddressesAsync())
                     .ReturnsAsync(storageResolvedAddresses.AsQueryable())
                     .ReturnsAsync(storageBatchResolvedAddresses.AsQueryable())
                     .ReturnsAsync(new List<ResolvedAddress>().AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(identifier);

            processingResolvedAddresses.ForEach(processingResolvedAddress =>
            {
                processingResolvedAddress.IsProcessing = true;
                processingResolvedAddress.RetryCount += 1;
                processingResolvedAddress.BatchReference = identifier;
            });

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
               processing.BulkModifyResolvedAddressesAsync(
                   It.Is(SameResolvedAddressListAs(processingResolvedAddresses))))
                    .ThrowsAsync(dependencyValidationException);

            failedProcessingResolvedAddresses.ForEach(failedProcessingResolvedAddress =>
            {
                failedProcessingResolvedAddress.IsProcessing = false;
                failedProcessingResolvedAddress.IsExported = false;
                failedProcessingResolvedAddress.IsProcessed = false;
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
            ValueTask<List<Guid>> exportResolvedAddressesTask =
                this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationServiceException =
               await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(
                   exportResolvedAddressesTask.AsTask);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
               .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Exactly(3));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(2));

            this.auditBrokerMock.Verify(service =>
                service.LogAsync(
                    auditType,
                    exportingAuditTitle,
                    exportingAuditMessage,
                    null,
                    identifier.ToString(),
                    "Information"),
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
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    innerResolvedAddressOrchestrationDependencyValidationException))),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualResolvedAddressOrchestrationServiceException))),
                        Times.Once);

            this.auditBrokerMock.Verify(service =>
                service.LogAsync(
                    auditType,
                    exportedAuditTitle,
                    exportedAuditMessage,
                    null,
                    identifier.ToString(),
                    "Information"),
                        Times.Never());

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
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
            storageResolvedAddresses.ForEach(address => address.IsProcessed = true);
            List<ResolvedAddress> storageBatchResolvedAddresses = randomResolvedAddresses.DeepClone();
            Guid identifier = Guid.NewGuid();
            string exportingAuditMessage = $"Exporting resolved addresses with correlation id {identifier}";
            string exportedAuditMessage = $"Exported resolved addresses with correlation id {identifier}";
            string exportingAuditTitle = "Exporting Resolved Addresses";
            string exportedAuditTitle = "Exported Resolved Addresses";
            string auditType = "Resolved Address Export";

            storageBatchResolvedAddresses.ForEach(pra =>
            {
                pra.BatchReference = identifier;
            });

            List<ResolvedAddress> processingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<ResolvedAddress> failedProcessingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<Exception> exceptions = new List<Exception>();

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
                service.RetrieveAllResolvedAddressesAsync())
                    .ReturnsAsync(storageResolvedAddresses.AsQueryable())
                    .ReturnsAsync(storageBatchResolvedAddresses.AsQueryable())
                    .ReturnsAsync(new List<ResolvedAddress>().AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(identifier);

            processingResolvedAddresses.ForEach(processingResolvedAddress =>
            {
                processingResolvedAddress.IsProcessing = true;
                processingResolvedAddress.RetryCount += 1;
                processingResolvedAddress.BatchReference = identifier;
            });

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(SameResolvedAddressListAs(processingResolvedAddresses))))
                        .ThrowsAsync(dependencyException);

            failedProcessingResolvedAddresses.ForEach(failedProcessingResolvedAddress =>
            {
                failedProcessingResolvedAddress.IsProcessing = false;
                failedProcessingResolvedAddress.IsExported = false;
                failedProcessingResolvedAddress.IsProcessed = false;
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
            ValueTask<List<Guid>> exportResolvedAddressesTask =
                this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationServiceException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(
                    exportResolvedAddressesTask.AsTask);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Exactly(3));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(2));

            this.auditBrokerMock.Verify(service =>
                service.LogAsync(
                    auditType,
                    exportingAuditTitle,
                    exportingAuditMessage,
                    null,
                    identifier.ToString(),
                    "Information"),
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
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    innerResolvedAddressOrchestrationDependencyException))),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualResolvedAddressOrchestrationServiceException))),
                        Times.Once);

            this.auditBrokerMock.Verify(service =>
                service.LogAsync(
                    auditType,
                    exportedAuditTitle,
                    exportedAuditMessage,
                    null,
                    identifier.ToString(),
                    "Information"),
                        Times.Never());

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnExportResolvedAddressErrorsInLoopAndLogItAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 2);
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            storageResolvedAddresses.ForEach(address => address.IsProcessed = true);
            List<ResolvedAddress> storageBatchResolvedAddresses = randomResolvedAddresses.DeepClone();
            Guid identifier = Guid.NewGuid();
            string exportingAuditMessage = $"Exporting resolved addresses with correlation id {identifier}";
            string exportedAuditMessage = $"Exported resolved addresses with correlation id {identifier}";
            string exportingAuditTitle = "Exporting Resolved Addresses";
            string exportedAuditTitle = "Exported Resolved Addresses";
            string auditType = "Resolved Address Export";

            storageBatchResolvedAddresses.ForEach(pra =>
            {
                pra.BatchReference = identifier;
            });

            List<ResolvedAddress> processingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<ResolvedAddress> failedProcessingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<Exception> exceptions = new List<Exception>();
            var serviceException = new Exception();

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
                service.RetrieveAllResolvedAddressesAsync())
                    .ReturnsAsync(storageResolvedAddresses.AsQueryable())
                    .ReturnsAsync(storageBatchResolvedAddresses.AsQueryable())
                    .ReturnsAsync(new List<ResolvedAddress>().AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(identifier);

            processingResolvedAddresses.ForEach(processingResolvedAddress =>
            {
                processingResolvedAddress.IsProcessing = true;
                processingResolvedAddress.RetryCount += 1;
                processingResolvedAddress.BatchReference = identifier;
            });

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(SameResolvedAddressListAs(processingResolvedAddresses))))
                        .ThrowsAsync(serviceException);

            failedProcessingResolvedAddresses.ForEach(failedProcessingResolvedAddress =>
            {
                failedProcessingResolvedAddress.IsProcessing = false;
                failedProcessingResolvedAddress.IsExported = false;
                failedProcessingResolvedAddress.IsProcessed = false;
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
            ValueTask<List<Guid>> exportResolvedAddressesTask =
                this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationServiceException =
               await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(
                   exportResolvedAddressesTask.AsTask);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
               .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Exactly(3));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(2));

            this.auditBrokerMock.Verify(service =>
                service.LogAsync(
                    auditType,
                    exportingAuditTitle,
                    exportingAuditMessage,
                    null,
                    identifier.ToString(),
                    "Information"),
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
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    innerResolvedAddressOrchestrationServiceException))),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualResolvedAddressOrchestrationServiceException))),
                        Times.Once);

            this.auditBrokerMock.Verify(service =>
                service.LogAsync(
                    auditType,
                    exportedAuditTitle,
                    exportedAuditMessage,
                    null,
                    identifier.ToString(),
                    "Information"),
                        Times.Never());

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}
