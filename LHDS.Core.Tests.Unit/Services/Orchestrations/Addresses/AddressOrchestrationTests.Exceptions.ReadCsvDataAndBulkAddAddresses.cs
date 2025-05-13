// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowAggregateExceptionOnReadCsvDataIfErrorsInLoadCallsAsync()
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
            string lpiCsvFilePath = "ID24.csv";
            string blpuCsvFilePath = "ID21.csv";
            string streetDescriptorCsvFilePath = "ID15.csv";
            int inputBatchSize = GetRandomNumber();
            int initialSkipCounter = 0;

            List<string> fileList = [
                dpaCsvFilePath,
                lpiCsvFilePath,
                blpuCsvFilePath,
                streetDescriptorCsvFilePath];

            string inputTempPath = GetRandomString();
            string inputSearchPattern = "*.csv";
            Guid randomGuid = Guid.NewGuid();
            Guid inputCorrelationId = randomGuid;
            Exception dpaException = new Exception();
            Exception lpiException = new Exception();
            Exception blpuException = new Exception();
            Exception streetDescriptorException = new Exception();

            this.fileBrokerMock.Setup(service =>
                service.GetListOfFilesAsync(inputTempPath, inputSearchPattern))
                    .ReturnsAsync(fileList);

            this.identifierBrokerMock.SetupSequence(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(inputCorrelationId)
                    .ThrowsAsync(lpiException);

            this.fileBrokerMock.Setup(service =>
                service.ReadLinesBatchAsync(dpaCsvFilePath, inputBatchSize, initialSkipCounter))
                    .ThrowsAsync(dpaException);

            this.fileBrokerMock.Setup(service =>
                service.ReadLinesBatchAsync(blpuCsvFilePath, inputBatchSize, initialSkipCounter))
                    .ThrowsAsync(blpuException);

            this.fileBrokerMock.Setup(service =>
                service.ReadLinesBatchAsync(streetDescriptorCsvFilePath, inputBatchSize, initialSkipCounter))
                    .ThrowsAsync(streetDescriptorException);

            Exception expectedDpaException = new Exception();
            Exception expectedLpiException = new Exception();
            Exception expectedBlpuException = new Exception();
            Exception expectedStreetDescriptorException = new Exception();

            List<Exception> expectedExceptions = [
                expectedDpaException,
                expectedLpiException,
                expectedBlpuException,
                expectedStreetDescriptorException];

            var expectedAggregateException =
                new AggregateException(
                    message: $"Unable to extract {expectedExceptions.Count} address files.",
                    expectedExceptions);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            ValueTask readCsvDataTask = service.ReadCsvDataAndBulkAddAddressesAsync(inputTempPath, inputBatchSize);

            AggregateException actualAggregateException =
                await Assert.ThrowsAsync<AggregateException>(
                    readCsvDataTask.AsTask);

            // Then
            actualAggregateException.Should().BeEquivalentTo(expectedAggregateException);

            this.fileBrokerMock.Verify(service =>
                service.GetListOfFilesAsync(inputTempPath, inputSearchPattern),
                    Times.Once());

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(2));

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import",
                    "Processing Address Files",
                     $"Starting processing of files in {inputTempPath}.",
                    inputTempPath,
                    inputCorrelationId.ToString()),
                        Times.Once);

            this.fileBrokerMock.Verify(service =>
                service.ReadLinesBatchAsync(dpaCsvFilePath, inputBatchSize, initialSkipCounter),
                    Times.Once);

            this.fileBrokerMock.Verify(service =>
                service.ReadLinesBatchAsync(blpuCsvFilePath, inputBatchSize, initialSkipCounter),
                    Times.Once);

            this.fileBrokerMock.Verify(service =>
                service.ReadLinesBatchAsync(streetDescriptorCsvFilePath, inputBatchSize, initialSkipCounter),
                    Times.Once);

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import",
                    "Processing Address Files",
                    $"Processing of files in {inputTempPath} is finished.",
                    inputTempPath,
                    inputCorrelationId.ToString()),
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

