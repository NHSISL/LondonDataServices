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
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldMapDPADataToAddressesAsync()
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

            string assembly = Assembly.GetExecutingAssembly().Location;
            string inputCsvFileName = "ShouldProcessZipFileWithOnlyCsvAddressesData.csv";

            string inputCsvFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                $"Resources/Services/Orchestrations/Addresses/{inputCsvFileName}");

            byte[] csvData = await File.ReadAllBytesAsync(inputCsvFilePath);
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

            List<Address> randomAddresses = CreateRandomAddresses(count: 2).ToList();
            List<Address> outputAddresses = randomAddresses.DeepClone();
            List<Address> expectedAddresses = outputAddresses.DeepClone();
            string stringRecords = string.Join(Environment.NewLine, filteredRecords);
            bool hasHeaderRecord = false;

            this.fileBrokerMock.Setup(service =>
                service.ReadFileAsync(inputCsvFilePath))
                    .ReturnsAsync(csvData);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<Address>(stringRecords, hasHeaderRecord, fieldMappings, true))
                    .ReturnsAsync(outputAddresses);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            List<Address> actualAddresses = await service.MapDPADataToAddressesAsync(inputCsvFilePath);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.fileBrokerMock.Verify(service =>
                service.ReadFileAsync(inputCsvFilePath),
                    Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<Address>(stringRecords, hasHeaderRecord, fieldMappings, true),
                    Times.Once());

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

