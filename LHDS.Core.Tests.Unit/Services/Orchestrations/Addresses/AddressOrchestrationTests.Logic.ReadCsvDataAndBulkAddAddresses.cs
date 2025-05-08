// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldReadCsvDataAndBulkAddAddressesAsync()
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

            string inputTempFolder = GetRandomString();
            string dpaCsvFilePath = "ID28_" + GetRandomString();
            string lpiCsvFilePath = "ID24_" + GetRandomString();
            string blpuCsvFilePath = "ID21_" + GetRandomString();
            string streetDescriptorCsvFilePath = "ID15_" + GetRandomString();
            int inputBatchSize = GetRandomNumber();
            Guid randomGuid = Guid.NewGuid();
            Guid inputCorrelationId = randomGuid;

            List<string> outputFileList = [
                dpaCsvFilePath,
                lpiCsvFilePath,
                blpuCsvFilePath,
                streetDescriptorCsvFilePath];

            this.fileBrokerMock.Setup(service =>
                service.GetListOfFilesAsync(inputTempFolder, "*.csv"))
                    .ReturnsAsync(outputFileList);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(inputCorrelationId);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessDPAAddressesAsync(dpaCsvFilePath, inputBatchSize))
                    .Returns(ValueTask.CompletedTask);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessLPIAddressesAsync(lpiCsvFilePath, inputBatchSize))
                    .Returns(ValueTask.CompletedTask);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessBLPUAddressesAsync(blpuCsvFilePath, inputBatchSize))
                    .Returns(ValueTask.CompletedTask);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessStreetDescriptorDataAsync(streetDescriptorCsvFilePath, inputBatchSize))
                    .Returns(ValueTask.CompletedTask);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            await service.ReadCsvDataAndBulkAddAddressesAsync(inputTempFolder, inputBatchSize);

            // Then
            this.fileBrokerMock.Verify(service =>
                service.GetListOfFilesAsync(inputTempFolder, "*.csv"),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import",
                    "Processing Address Files",
                     $"Starting processing of files in {inputTempFolder}.",
                    inputTempFolder,
                    inputCorrelationId.ToString()),
                        Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessDPAAddressesAsync(dpaCsvFilePath, inputBatchSize),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessLPIAddressesAsync(lpiCsvFilePath, inputBatchSize),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessBLPUAddressesAsync(blpuCsvFilePath, inputBatchSize),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessStreetDescriptorDataAsync(streetDescriptorCsvFilePath, inputBatchSize),
                    Times.Once);

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import",
                    "Processing Address Files",
                    $"Processing of files in {inputTempFolder} is finished.",
                    inputTempFolder,
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

