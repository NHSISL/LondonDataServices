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
        public async Task ShouldMapStreetDescriptorDataToAddressesAsync()
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
               record.StartsWith("15,") || record.StartsWith("\"15\",")).ToList();

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
            {
                { "USRN", 3 },
                { "StreetDescription", 4 },
                { "Locality", 5 },
                { "TownName", 6 },
            };

            List<StreetDescriptor> randomStreetDescriptors = CreateRandomStreetDescriptors(count: 2);
            List<StreetDescriptor> outputStreetDescriptors = randomStreetDescriptors.DeepClone();
            List<Address> expectedAddresses = [];

            foreach (StreetDescriptor streetDescriptor in outputStreetDescriptors)
            {
                Address address = new Address
                {
                    USRN = streetDescriptor.USRN,
                    Thoroughfare = streetDescriptor.StreetDescription,
                    DependentLocality = streetDescriptor.Locality,
                    PostTown = streetDescriptor.TownName
                };

                expectedAddresses.Add(address);
            }

            string stringRecords = string.Join(Environment.NewLine, filteredRecords);
            bool hasHeaderRecord = false;

            this.fileBrokerMock.Setup(service =>
                service.ReadFileAsync(inputCsvFilePath))
                    .ReturnsAsync(csvData);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<StreetDescriptor>(stringRecords, hasHeaderRecord, fieldMappings, true))
                    .ReturnsAsync(outputStreetDescriptors);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            List<Address> actualAddresses = await service.MapStreetDescriptorDataToAddressesAsync(inputCsvFilePath);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.fileBrokerMock.Verify(service =>
                service.ReadFileAsync(inputCsvFilePath),
                    Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<StreetDescriptor>(stringRecords, hasHeaderRecord, fieldMappings, true),
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

