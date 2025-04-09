// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
                this.loggingBrokerMock.Object)
            { CallBase = true };

            string inputTempFolder = GetRandomString();
            string dpaCsvFilePath = "ID28_" + GetRandomString();
            string lpiCsvFilePath = "ID24_" + GetRandomString();
            string blpuCsvFilePath = "ID21_" + GetRandomString();
            string streetDescriptorCsvFilePath = "ID15_" + GetRandomString();

            List<string> outputFileList = [
                dpaCsvFilePath,
                lpiCsvFilePath,
                blpuCsvFilePath,
                streetDescriptorCsvFilePath];

            this.fileBrokerMock.Setup(service =>
                service.GetListOfFilesAsync(inputTempFolder, "*.csv"))
                    .ReturnsAsync(outputFileList);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessDPAAddressesAsync(dpaCsvFilePath))
                    .Returns(ValueTask.CompletedTask);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessLPIAddressesAsync(lpiCsvFilePath))
                    .Returns(ValueTask.CompletedTask);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessBLPUAddressesAsync(blpuCsvFilePath))
                    .Returns(ValueTask.CompletedTask);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessStreetDescriptorDataAsync(streetDescriptorCsvFilePath))
                    .Returns(ValueTask.CompletedTask);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            await service.ReadCsvDataAndBulkAddAddressesAsync(inputTempFolder);

            // Then
            this.fileBrokerMock.Verify(service =>
                service.GetListOfFilesAsync(inputTempFolder, "*.csv"),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessDPAAddressesAsync(dpaCsvFilePath),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessLPIAddressesAsync(lpiCsvFilePath),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessBLPUAddressesAsync(blpuCsvFilePath),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessStreetDescriptorDataAsync(streetDescriptorCsvFilePath),
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

