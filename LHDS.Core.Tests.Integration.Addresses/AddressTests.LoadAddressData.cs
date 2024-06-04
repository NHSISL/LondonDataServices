// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using Xunit;

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task LoadAddressDataAsync()
        {
            //var filePath = @"Resources\Ordnance\0040176014-6414006-1.zip";
            //var filePath = @"Resources\Ordnance\0040176014-6414006-1SMALL.zip";
            var filePath = @"Resources\Ordnance\0040176014-6414006-1VERYSMALL.zip";

            // Given
            byte[] fileBytes = File.ReadAllBytes(filePath);
            FileInfo fi = new FileInfo(filePath);
            var fileName = fi.Name;

            // When
            List<Address> returnedAddresses =
                await addressClient.LoadAddressDataAsync(fileBytes, fileName);

            // Then

        }
    }
}
