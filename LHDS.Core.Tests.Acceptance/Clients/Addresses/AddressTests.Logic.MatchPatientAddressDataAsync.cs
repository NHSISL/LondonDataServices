// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldMatchPatientAddressDataAsync()
        {
            //Given
            byte[] uprnFile = Encoding.ASCII.GetBytes(GetRandomString());
            string fileName = GetRandomString();

            //When
            await this.addressClient.MatchPatientAddressDataAsync(uprnFile, fileName);

            //Then

        }

    }
}
