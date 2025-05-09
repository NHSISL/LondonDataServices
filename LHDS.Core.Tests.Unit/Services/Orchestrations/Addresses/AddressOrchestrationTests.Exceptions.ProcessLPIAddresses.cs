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
            int inputSkipCounter = 0;
            int inputBatchSize = this.batchSize;
            List<string> returnedStringList = CreateRandomStringList();
            Xeption lpiException = new Xeption();

            this.fileBrokerMock.Setup(broker =>
                broker.ReadLinesBatchAsync(lpiCsvFilePath, inputBatchSize, inputSkipCounter))
                    .ReturnsAsync(returnedStringList);

            addressOrchestrationServiceMock.Setup(service =>
                service.MapLPIDataToAddressesAsync(lpiCsvFilePath, inputBatchSize, inputSkipCounter))
                    .ThrowsAsync(lpiException);

            this.fileBrokerMock.Setup(broker =>
                broker.ReadLinesBatchAsync(lpiCsvFilePath, inputBatchSize, inputSkipCounter + inputBatchSize))
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

            this.fileBrokerMock.Verify(broker =>
                broker.ReadLinesBatchAsync(lpiCsvFilePath, inputBatchSize, inputSkipCounter),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapLPIDataToAddressesAsync(
                    lpiCsvFilePath,
                    inputBatchSize,
                    inputSkipCounter),
                        Times.Once);

            this.fileBrokerMock.Verify(broker =>
                broker.ReadLinesBatchAsync(lpiCsvFilePath, inputBatchSize, inputSkipCounter + inputBatchSize),
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

