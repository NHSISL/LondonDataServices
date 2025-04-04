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
using LHDS.Core.Models.Orchestrations.Addresses;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldMapBLPUDataToAddressesAsync()
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
               record.StartsWith("21,") || record.StartsWith("\"21\",")).ToList();

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
            {
                { "UPRN", 3 },
                { "LogicalStatus", 4 },
                { "StartDate", 15 },
                { "EndDate", 16 },
                { "PostCode", 20 },
            };

            List<BLPUAddress> randomBlpuAddresses = CreateRandomBLPUAddresses(count: 2);
            List<BLPUAddress> outputBlpuAddresses = randomBlpuAddresses.DeepClone();
            List<Address> expectedAddresses = [];

            foreach (BLPUAddress blpuAddress in outputBlpuAddresses)
            {
                Address address = new Address
                {
                    UPRN = blpuAddress.UPRN,
                    PostCode = blpuAddress.PostCode,
                };

                expectedAddresses.Add(address);
            }

            string stringRecords = string.Join(Environment.NewLine, filteredRecords);
            bool hasHeaderRecord = false;

            this.fileBrokerMock.Setup(service =>
                service.CheckIfFileExistsAsync(inputCsvFilePath))
                    .ReturnsAsync(true);

            this.fileBrokerMock.Setup(service =>
                service.ReadFileAsync(inputCsvFilePath))
                    .ReturnsAsync(csvData);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<BLPUAddress>(stringRecords, hasHeaderRecord, fieldMappings, true))
                    .ReturnsAsync(outputBlpuAddresses);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            List<Address> actualAddresses = await service.MapBLPUDataToAddressesAsync(inputCsvFilePath);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.fileBrokerMock.Verify(service =>
                service.CheckIfFileExistsAsync(inputCsvFilePath),
                    Times.Once);

            this.fileBrokerMock.Verify(service =>
                service.ReadFileAsync(inputCsvFilePath),
                    Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<BLPUAddress>(stringRecords, hasHeaderRecord, fieldMappings, true),
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
