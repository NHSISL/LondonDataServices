// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldImportOrdinanceAddressesAsync()
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

            string assembly = Assembly.GetExecutingAssembly().Location;
            string zipFileName = "ShouldProcessZipFileWithZippedCsvAddressesData.zip";

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                $"Resources/Services/Orchestrations/Addresses/{zipFileName}");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            Stream inputStream = new MemoryStream(inputData);
            string inputFileName = zipFileName;
            string randomTempPath = Path.GetTempPath();
            string ordinanceTempFolder = Path.Combine(randomTempPath, "OrdinanceData");
            int batchSize = 120000;

            string ordinanceTempCsvFile =
                Path.Combine(ordinanceTempFolder, "ShouldProcessZipFileWithOnlyCsvAddressesData.csv");

            this.fileBrokerMock.Setup(service =>
                service.GetTempPath())
                    .ReturnsAsync(randomTempPath);

            this.fileBrokerMock.Setup(service =>
                service.CheckIfDirectoryExistsAsync(ordinanceTempFolder))
                    .ReturnsAsync(false);

            addressOrchestrationServiceMock.Setup(service =>
                service.UnZipAndExtractAsync(inputStream, ordinanceTempFolder))
                    .Returns(ValueTask.CompletedTask);

            addressOrchestrationServiceMock.Setup(service =>
                service.ReadCsvDataAndBulkAddAddressesAsync(ordinanceTempFolder, batchSize))
                    .Returns(ValueTask.CompletedTask);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            await service.BulkAddAddressesAsync(input: inputStream, zipFileName);

            // Then
            this.fileBrokerMock.Verify(service =>
                service.GetTempPath(),
                    Times.Once);

            this.fileBrokerMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(ordinanceTempFolder),
                    Times.Once);

            this.fileBrokerMock.Verify(service =>
                service.CreateDirectoryAsync(ordinanceTempFolder),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.UnZipAndExtractAsync(inputStream, ordinanceTempFolder),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ReadCsvDataAndBulkAddAddressesAsync(ordinanceTempFolder, batchSize),
                    Times.Once);

            this.fileBrokerMock.Verify(service =>
                service.DeleteDirectoryAsync(ordinanceTempFolder, true),
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

