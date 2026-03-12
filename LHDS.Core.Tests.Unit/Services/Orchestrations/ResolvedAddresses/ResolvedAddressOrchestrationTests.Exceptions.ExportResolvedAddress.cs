// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Audits;
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

        [Fact]
        public async Task ShouldThrowAggregateExceptionOnExportResolvedAddressIfErrorsInLoopAndLogItAsync()
        {
            // Given
            Xeption someException = new Xeption(GetRandomString());
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses(
                count: 2,
                dateTimeOffset: randomDateTimeOffset,
                isExported: false,
                isProcessed: true,
                isProcessing: false,
                retryCount: 1);

            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            List<ResolvedAddress> failedResolvedAddresses = storageResolvedAddresses.DeepClone();

            failedResolvedAddresses.ForEach(resolvedAddress =>
            {
                resolvedAddress.IsProcessing = false;
                resolvedAddress.IsExported = false;
            });

            List<Exception> exceptions = new List<Exception>();

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
                service.RetrieveAllResolvedAddressesAsync())
                    .ReturnsAsync(storageResolvedAddresses.AsQueryable())
                    .ThrowsAsync(someException)
                    .ReturnsAsync(storageResolvedAddresses.AsQueryable())
                    .ReturnsAsync(new List<ResolvedAddress>().AsQueryable());

            Guid identifier = Guid.NewGuid();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(identifier);

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(SameResolvedAddressListAs(failedResolvedAddresses))))
                        .Returns(ValueTask.CompletedTask);

            exceptions.Add(someException);

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
                    Times.Once);

            this.auditBrokerMock.Verify(service =>
                service.BulkLogAsync(It.IsAny<List<Audit>>()),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(Valid8.SameObjectAs<List<ResolvedAddress>>(
                        failedResolvedAddresses,
                        output,
                        "Second bulk modify:"))),
                        Times.Once);

            //this.loggingBrokerMock.Verify(broker =>
            //    broker.LogErrorAsync(It.Is(SameExceptionAs(
            //        innerResolvedAddressOrchestrationDependencyException))),
            //            Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}
