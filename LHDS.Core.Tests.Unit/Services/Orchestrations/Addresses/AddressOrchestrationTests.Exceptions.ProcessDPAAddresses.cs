// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowAggregateExceptionOnProcessDPAIfErrorsInMapCallsAsync()
        {
            // Given
            var addressOrchestrationServiceMock = new Mock<AddressOrchestrationService>
               (this.addressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.fileBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.identifierBrokerMock.Object)
            { CallBase = true };

            string dpaCsvFilePath = "ID28.csv";
            Guid randomGuid = Guid.NewGuid();
            Guid inputCorrelationId = randomGuid;
            int inputSkipCounter = 0;
            int inputBatchSize = this.batchSize;
            Xeption dpaException = new Xeption();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(inputCorrelationId);

            addressOrchestrationServiceMock.Setup(service =>
                service.MapDPADataToAddressesAsync(dpaCsvFilePath, inputBatchSize, inputSkipCounter))
                    .ThrowsAsync(dpaException);

            addressOrchestrationServiceMock.Setup(service =>
                service.MapDPADataToAddressesAsync(dpaCsvFilePath, inputBatchSize, inputSkipCounter + inputBatchSize))
                    .ReturnsAsync([]);

            Xeption expectedDpaException = new Xeption();

            expectedDpaException.AddData(
                $"DpaExtractionError in batch between lines {inputSkipCounter} " +
                $"and {inputSkipCounter + inputBatchSize}.",
                dpaCsvFilePath);

            List<Exception> expectedExceptions = [expectedDpaException];

            var expectedAggregateException =
                new AggregateException(
                    message: $"Errors occurred during loading of {expectedExceptions.Count} batches.",
                    expectedExceptions);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            ValueTask readCsvDataTask = service.ProcessDPAAddressesAsync(dpaCsvFilePath);

            AggregateException actualAggregateException =
                await Assert.ThrowsAsync<AggregateException>(
                    readCsvDataTask.AsTask);

            // Then
            actualAggregateException.Should().BeEquivalentTo(expectedAggregateException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import - DPA Processing",
                    "Processing DPA File",
                    $"Starting processing file {dpaCsvFilePath}.",
                    dpaCsvFilePath,
                    inputCorrelationId.ToString()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Starting processing file {dpaCsvFilePath}."),
                        Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapDPADataToAddressesAsync(
                    dpaCsvFilePath,
                    inputBatchSize,
                    inputSkipCounter),
                        Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapDPADataToAddressesAsync(
                    dpaCsvFilePath,
                    inputBatchSize,
                    inputSkipCounter + inputBatchSize),
                        Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessDPAAddressesAsync(dpaCsvFilePath, this.batchSize),
                    Times.Once);

            addressOrchestrationServiceMock.VerifyNoOtherCalls();

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import - DPA Processing",
                    "Processing DPA File",
                    $"Finished processing file {dpaCsvFilePath}.",
                    dpaCsvFilePath,
                    inputCorrelationId.ToString()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Finished processing file {dpaCsvFilePath}."),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}

