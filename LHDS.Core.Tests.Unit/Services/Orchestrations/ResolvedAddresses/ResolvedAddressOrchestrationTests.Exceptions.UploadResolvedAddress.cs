// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
            ShouldThrowAggregateDependencyValidationExceptionOnProcessResolvedAddressesIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            List<Exception> exceptions = new List<Exception>();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            string ouputCsv = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(ouputCsv);
            Guid batchReference = Guid.NewGuid();
            string fileName = $"{batchReference}.csv";
            string container = blobContainers.Addresses;

            Document inputDocument = new Document
            {
                DocumentData = new MemoryStream(inputData),
                FileName = fileName
            };

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses()).
                    Returns(storageResolvedAddresses.AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(batchReference);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapObjectToCsvAsync(
                    It.IsAny<List<ResolvedAddressReturn>>(), It.IsAny<bool>(), null, It.IsAny<bool>()))
                        .ReturnsAsync(ouputCsv);

            foreach (ResolvedAddress resolvedAddress in storageResolvedAddresses)
            {
                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset()).Returns(dateTimeOffset);

                this.resolvedAddressProcessingServiceMock.Setup(service =>
                    service.ModifyResolvedAddressAsync(It.IsAny<ResolvedAddress>()))
                        .ThrowsAsync(dependencyValidationException);

                var resolvedAddressOrchestrationDependencyValidationException =
                    new ResolvedAddressOrchestrationDependencyValidationException(
                        message: "Resolved address orchestration dependency validation error occurred, " +
                            "please try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                exceptions.Add(resolvedAddressOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    message: $"Unable to modify resolved address for {exceptions.Count} resolved addresses " +
                        $"in batch: {batchReference}",
                    exceptions);

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            // When
            ValueTask<Guid?> uploadResolvedAddressTask =
                this.resolvedAddressOrchestrationService.UploadResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException
                actualResolvedAddressOrchestrationServiceException =
                    await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(async () =>
                        await uploadResolvedAddressTask);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapObjectToCsvAsync(
                    It.IsAny<List<ResolvedAddressReturn>>(), It.IsAny<bool>(), null, It.IsAny<bool>()),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(storageResolvedAddresses.Count));

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.ModifyResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Exactly(storageResolvedAddresses.Count));

            var resolvedAddressOrchestrationDependencyValidationLoggingException =
                new ResolvedAddressOrchestrationDependencyValidationException(
                    message: "Resolved address orchestration dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    resolvedAddressOrchestrationDependencyValidationLoggingException))),
                        Times.Exactly(randomResolvedAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualResolvedAddressOrchestrationServiceException))),
                        Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task
            ShouldThrowAggregateDependencyExceptionOnProcessResolvedAddressesIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            List<Exception> exceptions = new List<Exception>();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            string ouputCsv = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(ouputCsv);
            Guid batchReference = Guid.NewGuid();
            string fileName = $"{batchReference}.csv";
            string container = blobContainers.Addresses;

            Document inputDocument = new Document
            {
                DocumentData = new MemoryStream(inputData),
                FileName = fileName
            };

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Returns(storageResolvedAddresses.AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(batchReference);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapObjectToCsvAsync(
                    It.IsAny<List<ResolvedAddressReturn>>(), It.IsAny<bool>(), null, It.IsAny<bool>()))
                        .ReturnsAsync(ouputCsv);

            foreach (ResolvedAddress resolvedAddress in storageResolvedAddresses)
            {
                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Returns(dateTimeOffset);

                this.resolvedAddressProcessingServiceMock.Setup(service =>
                    service.ModifyResolvedAddressAsync(It.IsAny<ResolvedAddress>()))
                        .ThrowsAsync(dependencyException);

                var resolvedAddressOrchestrationDependencyException =
                    new ResolvedAddressOrchestrationDependencyException(
                        message: "Resolved address orchestration dependency error occurred, please try again.",
                        innerException: dependencyException.InnerException as Xeption);

                exceptions.Add(resolvedAddressOrchestrationDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    message: $"Unable to modify resolved address for {exceptions.Count} resolved addresses " +
                        $"in batch: {batchReference}",
                    exceptions);

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            // When
            ValueTask<Guid?> uploadResolvedAddressTask =
                 this.resolvedAddressOrchestrationService.UploadResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationServiceException =
                    await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(async () =>
                        await uploadResolvedAddressTask);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapObjectToCsvAsync(
                    It.IsAny<List<ResolvedAddressReturn>>(), It.IsAny<bool>(), null, It.IsAny<bool>()),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(storageResolvedAddresses.Count));

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.ModifyResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Exactly(storageResolvedAddresses.Count));

            var resolvedAddressOrchestrationDependencyLoggingException =
                new ResolvedAddressOrchestrationDependencyException(
                    message: "Resolved address orchestration dependency error occurred, " +
                        "please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    resolvedAddressOrchestrationDependencyLoggingException))),
                        Times.Exactly(randomResolvedAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualResolvedAddressOrchestrationServiceException))),
                        Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnProcessResolvedAddressesIfErrorsInLoopAndLogItAsync()
        {
            // Given
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            List<Exception> exceptions = new List<Exception>();
            var serviceException = new Exception();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            string ouputCsv = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(ouputCsv);
            Guid batchReference = Guid.NewGuid();
            string fileName = $"{batchReference}.csv";
            string container = blobContainers.Addresses;

            Document inputDocument = new Document
            {
                DocumentData = new MemoryStream(inputData),
                FileName = fileName
            };

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses()).
                    Returns(storageResolvedAddresses.AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(batchReference);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapObjectToCsvAsync(
                    It.IsAny<List<ResolvedAddressReturn>>(), It.IsAny<bool>(), null, It.IsAny<bool>()))
                        .ReturnsAsync(ouputCsv);

            var innerFailedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var innerResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: innerFailedResolvedAddressOrchestrationServiceException);

            foreach (ResolvedAddress resolvedAddress in storageResolvedAddresses)
            {
                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Returns(dateTimeOffset);

                this.resolvedAddressProcessingServiceMock.Setup(service =>
                    service.ModifyResolvedAddressAsync(It.IsAny<ResolvedAddress>()))
                        .ThrowsAsync(serviceException);

                exceptions.Add(innerResolvedAddressOrchestrationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    message: $"Unable to modify resolved address for {exceptions.Count} resolved addresses " +
                        $"in batch: {batchReference}",
                    exceptions);

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            // When
            ValueTask<Guid?> uploadResolvedAddressTask =
                  this.resolvedAddressOrchestrationService.UploadResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationServiceException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(async () =>
                    await uploadResolvedAddressTask);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
               service.RetrieveAllResolvedAddresses(),
                   Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapObjectToCsvAsync(
                    It.IsAny<List<ResolvedAddressReturn>>(), It.IsAny<bool>(), null, It.IsAny<bool>()),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(storageResolvedAddresses.Count));

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.ModifyResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Exactly(storageResolvedAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    innerResolvedAddressOrchestrationServiceException))),
                        Times.Exactly(randomResolvedAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressOrchestrationServiceException))),
                        Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationExceptionOnUploadIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedResolvedAddressOrchestrationDependencyValidationException =
                new ResolvedAddressOrchestrationDependencyValidationException(
                    message: "Resolved address orchestration dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<Guid?> documentAddTask =
                this.resolvedAddressOrchestrationService.UploadResolvedAddressesAsync();

            ResolvedAddressOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyValidationException>(
                    documentAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyValidationException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationDependencyValidationException))),
                         Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnUploadIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedResolvedAddressOrchestrationDependencyException =
                new ResolvedAddressOrchestrationDependencyException(
                    message: "Resolved address orchestration dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(dependencyException);

            // when
            ValueTask<Guid?> documentAddTask =
                this.resolvedAddressOrchestrationService.UploadResolvedAddressesAsync();

            ResolvedAddressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyException>(documentAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationDependencyException))),
                         Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnUploadIfServiceErrorOccursAsync()
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
            ValueTask<Guid?> uploadResolvedAddressTask =
                 this.resolvedAddressOrchestrationService.UploadResolvedAddressesAsync();

            ResolvedAddressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(
                    uploadResolvedAddressTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationServiveException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationServiveException))),
                         Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
