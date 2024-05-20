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
            // Given
            byte[] fileBytes =
                File.ReadAllBytes(
                    //@"Resources\0040176014-6414006-1.zip");
                    @"Resources\Ordnance\TQ4065.csv");

            FileInfo fi =
                new FileInfo(
                    //@"Resources\0040176014-6414006-1.zip");
                    @"Resources\Ordnance\TQ4065.csv");

            var fileName = fi.Name;

            // When
            List<Address> returnedAddresses =
                await addressClient.LoadAddressDataAsync(fileBytes, fileName);

            // Then

        }
    }
}
