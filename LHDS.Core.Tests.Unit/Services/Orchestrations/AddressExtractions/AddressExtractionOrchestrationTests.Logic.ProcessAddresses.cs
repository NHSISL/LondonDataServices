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
using LHDS.Core.Extensions.Addresses;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
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

            AddressNormalisation addressNormalisation = new AddressNormalisation
            {
                PostalAddress = GetRandomString(),
                JsonPostalAddress = GetRandomString()
            };

            var storageAddresses = outputAddresses.AsQueryable();

            foreach (Address address in outputAddresses)
            {
                Address inputAddress = address;
                string addressString = address.GetFormattedAddress();
                Address? maybeAddress = storageAddresses.First(item => item.Id == address.Id);

                this.addressProcessingServiceMock.Setup(service =>
                    service.RetrieveAllAddresses())
                        .Returns(storageAddresses);

                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Returns(randomDateTimeOffset);

                inputAddress.Id = maybeAddress.Id;
                inputAddress.UpdatedBy = "System";
                inputAddress.UpdatedDate = randomDateTimeOffset;

                Address storageInputAddress = inputAddress.DeepClone();

                this.addressProcessingServiceMock.Setup(service =>
                    service.ModifyOrAddAddressAsync(inputAddress))
                        .ReturnsAsync(storageInputAddress);

                this.addressNormalisationProcessingServiceMock.Setup(service =>
                    service.GetNormalisedAddress(addressString))
                        .ReturnsAsync(addressNormalisation);

                storageInputAddress.PostalAddress = addressNormalisation.PostalAddress;
                storageInputAddress.JsonPostalAddress = addressNormalisation.JsonPostalAddress;
                storageInputAddress.IsErrored = false;

                Address updatedOutputAddress = storageInputAddress.DeepClone();
                updatedOutputAddress.UpdatedBy = "System";
                updatedOutputAddress.UpdatedDate = randomDateTimeOffset;

                this.addressProcessingServiceMock.Setup(service =>
                    service.ModifyOrAddAddressAsync(storageInputAddress))
                        .ReturnsAsync(updatedOutputAddress);

                expectedAddresses.Add(storageInputAddress);
            }

            // When
            List<Address> actualAddresses = await this.addressExtractionOrchestrationService
                .ProcessAddressesAsync(inputData, inputFilename);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
                options.Excluding(address => address.Id));

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<Address>(stringRecords, hasHeaderRecord, fieldMappings),
                    Times.Once());

            var verifyStorageAddresses = randomAddresses.AsQueryable();

            foreach (Address verifyAddress in randomAddresses)
            {
                Address verifyInputAddress = verifyAddress.DeepClone();
                string verifyAddressString = verifyAddress.GetFormattedAddress();
                Address? maybeAddress = verifyStorageAddresses.First(item => item.Id == verifyAddress.Id);

                this.addressProcessingServiceMock.Verify(service =>
                    service.RetrieveAllAddresses(), Times.Once);

                verifyInputAddress.Id = maybeAddress.Id;
                verifyInputAddress.UpdatedBy = "System";
                verifyInputAddress.UpdatedDate = randomDateTimeOffset;

                this.addressProcessingServiceMock.Verify(service =>
                    service.ModifyOrAddAddressAsync(It.Is(SameAddressAs(verifyInputAddress))),
                        Times.Once);

                this.addressNormalisationProcessingServiceMock.Verify(service =>
                    service.GetNormalisedAddress(verifyAddressString),
                        Times.Once);

                Address storageInputAddress = verifyInputAddress.DeepClone();
                storageInputAddress.PostalAddress = addressNormalisation.PostalAddress;
                storageInputAddress.JsonPostalAddress = addressNormalisation.JsonPostalAddress;
                storageInputAddress.IsErrored = false;

                this.auditBrokerMock.Verify(broker =>
                    broker.LogInformation(
                        "Address",
                        "Successfully extracted address from Ordinance Database",
                        $"Successfully extracted address with id: {storageInputAddress.Id} from file: {inputFilename}",
                        inputFilename,
                        storageInputAddress.Id),
                            Times.Once);

                Address updatedVerifyOutputAddress = storageInputAddress.DeepClone();
                updatedVerifyOutputAddress.UpdatedBy = "System";
                updatedVerifyOutputAddress.UpdatedDate = randomDateTimeOffset;

                this.addressProcessingServiceMock.Verify(service =>
                    service.ModifyOrAddAddressAsync(It.Is(SameAddressAs(updatedVerifyOutputAddress))),
                        Times.Once);
            }

            this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Exactly(2));

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

