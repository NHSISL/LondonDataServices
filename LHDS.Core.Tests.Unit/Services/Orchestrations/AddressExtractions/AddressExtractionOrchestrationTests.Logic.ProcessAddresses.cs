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

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldProcessAddressesAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();
            int randomItems = 1; // GetRandomNumber();
            string inputFilename = GetRandomString();
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessZipFileWithZippedCsvAddressesData.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

            string csvFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessZipFileWithOnlyCsvAddressesData.csv");

            byte[] csvData = await File.ReadAllBytesAsync(csvFilePath);
            string stringData = Encoding.UTF8.GetString(csvData);
            List<string> records = stringData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<string> filteredRecords = records.Where(record =>
               record.StartsWith("28,") || record.StartsWith("\"28\",")).ToList();

            string stringRecords = string.Join(Environment.NewLine, filteredRecords);
            bool hasHeaderRecord = false;

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

            List<Address> randomAddresses = CreateRandomAddresses(count: randomItems).ToList();
            List<Address> outputAddresses = randomAddresses.DeepClone();

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<Address>(stringRecords, hasHeaderRecord, fieldMappings))
                    .ReturnsAsync(outputAddresses);

            List<Address> expectedAddresses = outputAddresses.DeepClone();

            // When
            List<Address> actualAddresses = await this.addressExtractionOrchestrationService
                .ProcessAddressesAsync(inputData, inputFilename);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
                options
                    .Excluding(address => address.Id)
                    .Excluding(address => address.CreatedBy)
                    .Excluding(address => address.CreatedDate)
                    .Excluding(address => address.UpdatedBy)
                    .Excluding(address => address.UpdatedDate));

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<Address>(stringRecords, hasHeaderRecord, fieldMappings),
                    Times.Once());

            this.addressProcessingServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(outputAddresses, inputFilename),
                    Times.Once);

            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}

