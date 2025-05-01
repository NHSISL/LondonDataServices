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
        public async Task ShouldThrowAggregateExceptionOnProcessStreetDescriptorsIfErrorsInMapCallsAsync()
        {
            // Given
            var addressOrchestrationServiceMock = new Mock<AddressOrchestrationService>
               (this.addressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.fileBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            string streetDescriptorsCsvFilePath = "ID15.csv";
            int inputSkipCounter = 0;
            int inputBatchSize = this.batchSize;
            List<string> returnedStringList = CreateRandomStringList();
            Xeption streetDescriptorsException = new Xeption();

            this.fileBrokerMock.Setup(broker =>
                broker.ReadLinesBatchAsync(streetDescriptorsCsvFilePath, inputBatchSize, inputSkipCounter))
                    .ReturnsAsync(returnedStringList);

            addressOrchestrationServiceMock.Setup(service =>
                service.MapStreetDescriptorDataToAddressesAsync(streetDescriptorsCsvFilePath, inputBatchSize, inputSkipCounter))
                    .ThrowsAsync(streetDescriptorsException);

            this.fileBrokerMock.Setup(broker =>
                broker.ReadLinesBatchAsync(streetDescriptorsCsvFilePath, inputBatchSize, inputSkipCounter + inputBatchSize))
                    .ReturnsAsync([]);

            Xeption expectedBlpuException = new Xeption();
            expectedBlpuException.AddData(
                $"StreetDescriptorsExtractionError in batch between lines {inputSkipCounter} " +
                $"and {inputSkipCounter + inputBatchSize}.",
                streetDescriptorsCsvFilePath);

            List<Exception> expectedExceptions = [expectedBlpuException];

            var expectedAggregateException =
                new AggregateException(
                    message: $"Errors occurred during loading of {expectedExceptions.Count} batches.",
                    expectedExceptions);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            ValueTask readCsvDataTask = service.ProcessStreetDescriptorDataAsync(streetDescriptorsCsvFilePath);

            AggregateException actualAggregateException =
                await Assert.ThrowsAsync<AggregateException>(
                    readCsvDataTask.AsTask);

            // Then
            actualAggregateException.Should().BeEquivalentTo(expectedAggregateException);

            this.fileBrokerMock.Verify(broker =>
                broker.ReadLinesBatchAsync(streetDescriptorsCsvFilePath, inputBatchSize, inputSkipCounter),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapStreetDescriptorDataToAddressesAsync(
                    streetDescriptorsCsvFilePath,
                    inputBatchSize,
                    inputSkipCounter),
                        Times.Once);

            this.fileBrokerMock.Verify(broker =>
                broker.ReadLinesBatchAsync(streetDescriptorsCsvFilePath, inputBatchSize, inputSkipCounter + inputBatchSize),
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

