// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldLoadAddressDataAsync()
        {
            // Given
            string inputFilename = GetRandomString();
            string assembly = Assembly.GetExecutingAssembly().Location;
            char separator = Path.DirectorySeparatorChar;

            string projectRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(assembly),
                $"..{separator}..{separator}.."));

            string inputFilePath = Path.Combine(
                projectRoot,
                $"Resource{separator}Clients{separator}Address{separator}" +
                "ShouldProcessZipFileWithZippedCsvAddressesData.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            Stream inputStream = new MemoryStream(inputData);

            string csvFilePath = Path.Combine(
                projectRoot,
                $"Resource{separator}Clients{separator}Address{separator}" +
                "ShouldProcessCsvAddressesSetup.csv");

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

            List<Address> expectedListAddresses =
                await this.csvHelperBroker.MapCsvToObjectAsync<Address>(stringRecords, hasHeaderRecord, fieldMappings);

            // When
            await this.addressClient.LoadAddressDataAsync(
                inputStream,
                "ShouldProcessZipFileWithZippedCsvAddressesData.zip");

            // Then
            IQueryable<Address> retrievedListAddresses = this.addressService.RetrieveAllAddresses();

            foreach (Address expectedAddress in expectedListAddresses)
            {
                Address retrievedAddress =
                    retrievedListAddresses.Where(address => address.UPRN == expectedAddress.UPRN).FirstOrDefault();

                await this.addressService.RemoveAddressByIdAsync(retrievedAddress.Id);
            }
        }
    }
}


