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
        public async Task ShouldThrowAggregateExceptionOnProcessLPIIfErrorsInMapCallsAsync()
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

            string lpiCsvFilePath = "ID21.csv";
            Guid randomGuid = Guid.NewGuid();
            Guid inputCorrelationId = randomGuid;
            int inputSkipCounter = 0;
            int inputBatchSize = this.batchSize;
            Xeption lpiException = new Xeption();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(inputCorrelationId);

            addressOrchestrationServiceMock.Setup(service =>
                service.MapLPIDataToAddressesAsync(lpiCsvFilePath, inputBatchSize, inputSkipCounter))
                    .ThrowsAsync(lpiException);

            addressOrchestrationServiceMock.Setup(service =>
                service.MapLPIDataToAddressesAsync(lpiCsvFilePath, inputBatchSize, inputSkipCounter + inputBatchSize))
                    .ReturnsAsync([]);

            Xeption expectedLpiException = new Xeption();

            expectedLpiException.AddData(
                $"LpiExtractionError in batch between lines {inputSkipCounter} " +
                $"and {inputSkipCounter + inputBatchSize}.",
                lpiCsvFilePath);

            List<Exception> expectedExceptions = [expectedLpiException];

            var expectedAggregateException =
                new AggregateException(
                    message: $"Errors occurred during loading of {expectedExceptions.Count} batches.",
                    expectedExceptions);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            ValueTask readCsvDataTask = service.ProcessLPIAddressesAsync(lpiCsvFilePath);

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
                    "Address Import - LPI Processing",
                    "Processing LPI File",
                    $"Starting processing file {lpiCsvFilePath}.",
                    lpiCsvFilePath,
                    inputCorrelationId.ToString()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Starting processing file {lpiCsvFilePath}."),
                        Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapLPIDataToAddressesAsync(
                    lpiCsvFilePath,
                    inputBatchSize,
                    inputSkipCounter),
                        Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapLPIDataToAddressesAsync(
                    lpiCsvFilePath,
                    inputBatchSize,
                    inputSkipCounter + inputBatchSize),
                        Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessLPIAddressesAsync(lpiCsvFilePath, this.batchSize),
                    Times.Once);

            addressOrchestrationServiceMock.VerifyNoOtherCalls();

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import - LPI Processing",
                    "Processing LPI File",
                    $"Finished processing file {lpiCsvFilePath}.",
                    lpiCsvFilePath,
                    inputCorrelationId.ToString()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Finished processing file {lpiCsvFilePath}."),
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

