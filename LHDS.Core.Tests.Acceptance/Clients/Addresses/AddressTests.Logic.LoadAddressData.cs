// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
            string assembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            char separator = Path.DirectorySeparatorChar;

            string inputFilePath = Path.Combine(
                assembly,
                $"Resource{separator}Clients{separator}Address{separator}" +
                    "ShouldProcessZipFileWithAddressCsvFiles.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath, TestContext.Current.CancellationToken);
            Stream inputStream = new MemoryStream(inputData);

            string csvFolderPath = Path.Combine(
                assembly,
                $"Resource{separator}Clients{separator}Address{separator}" +
                "AddressCsvFiles");

            List<Address> expectedAddresses = GetExpectedAddresses();

            // When
            await this.addressClient.LoadAddressDataAsync(
                inputStream,
                "ShouldProcessZipFileWithAddressCsvFiles.zip");

            // Then
            IQueryable<Address> retrievedListAddresses = await this.addressService.RetrieveAllAddressesAsync();

            foreach (Address expectedAddress in expectedAddresses)
            {
                Address retrievedAddress =
                    retrievedListAddresses.Where(address => address.UPRN == expectedAddress.UPRN).FirstOrDefault();

                await this.addressService.RemoveAddressByIdAsync(retrievedAddress.Id);
            }
        }
    }
}


