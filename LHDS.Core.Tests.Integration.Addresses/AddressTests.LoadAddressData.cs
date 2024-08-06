// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task LoadAddressDataAsync()
        {
            string inputFilename = "pow.csv";
            //var filePath = @"Resources\Ordnance\0040176014-6414006-1.zip";
            //var filePath = @"Resources\Ordnance\0040176014-6414006-1SMALL.zip";
            var filePath = @"Resources\Ordnance\0040176014-6414006-1VERYSMALL.zip";

            // Given
            byte[] inputData = await File.ReadAllBytesAsync(filePath);
            Stream inputStream = new MemoryStream(inputData);

            // When
            //List<Address> returnedAddresses =
            //    await addressClient.LoadAddressDataAsync(inputStream, inputFilename);

            // Then

        }
    }
}
