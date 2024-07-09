// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
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
            string assembly = Assembly.GetExecutingAssembly().Location;
            string zipFileName = "ShouldProcessZipFileWithZippedCsvAddressesData.zip";
            string csvFileName = "ShouldProcessZipFileWithOnlyCsvAddressesData.zip";

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                $"Resources/Services/Orchestrations/Addresses/{zipFileName}");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            Stream inputStream = new MemoryStream(inputData);

            string csvFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                $"Resources/Services/Orchestrations/Addresses/{csvFileName}");

            byte[] csvData = await File.ReadAllBytesAsync(csvFilePath);
            string stringData = Encoding.UTF8.GetString(csvData);
            List<string> records = stringData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<string> filteredRecords = records.Where(record =>
               record.StartsWith("28,") || record.StartsWith("\"28\",")).ToList();

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { "UPRN", 3 },
                    { "UPSN", 4 },
                    { "OrganisationName", 5 },
                    { "DepartmentName", 6 },
                    { "SubBuildingName", 7 },
                    { "BuildingName", 8 },
                    { "BuildingNumber", 9 },
                    { "DependentThoroughfare", 10 },
                    { "Thoroughfare", 11 },
                    { "DoubleDependentLocality", 12 },
                    { "DependentLocality", 13 },
                    { "PostTown", 14 },
                    { "PostCode", 15 }
                };

            Guid randomId = Guid.NewGuid();
            List<Address> randomAddresses = CreateRandomAddresses(count: 1).ToList();
            List<Address> outputAddresses = randomAddresses.DeepClone();
            string stringRecords = string.Join(Environment.NewLine, filteredRecords);
            bool hasHeaderRecord = false;
            string inputFileName = zipFileName;
            string randomTempPath = Path.GetTempPath();
            string ordinanceTempFolder = Path.Combine(randomTempPath, "OrdinanceData");

            string ordinanceTempCsvFile =
                Path.Combine(ordinanceTempFolder, "ShouldProcessZipFileWithOnlyCsvAddressesData.csv");

            this.fileBrokerMock.Setup(service =>
                service.GetTempPath())
                    .ReturnsAsync(randomTempPath);

            this.fileBrokerMock.Setup(service =>
                service.CheckIfDirectoryExistsAsync(ordinanceTempFolder))
                    .ReturnsAsync(false);

            this.fileBrokerMock.Setup(service =>
                service.GetListOfFilesAsync(ordinanceTempFolder, "*.csv"))
                    .ReturnsAsync(new List<string> { ordinanceTempCsvFile });

            this.fileBrokerMock.Setup(service =>
                service.ReadFileAsync(ordinanceTempCsvFile))
                    .ReturnsAsync(csvData);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<Address>(stringRecords, hasHeaderRecord, fieldMappings))
                    .ReturnsAsync(outputAddresses);

            this.identifierBrokerMock.Setup(service =>
                service.GetIdentifier())
                    .Returns(randomId);

            // When
            await this.addressOrchestrationService
                .BulkAddAddressesAsync(input: inputStream, zipFileName);

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

            this.fileBrokerMock.Verify(service =>
                service.GetListOfFilesAsync(ordinanceTempFolder, "*.csv"),
                    Times.Once);

            this.fileBrokerMock.Verify(service =>
                service.ReadFileAsync(ordinanceTempCsvFile),
                    Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<Address>(stringRecords, hasHeaderRecord, fieldMappings),
                    Times.Once());

            this.addressProcessingServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(outputAddresses, ordinanceTempCsvFile),
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

