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
        [Fact(skip = 'Will fix in another PR')]
        public async Task AddressMatcherDataAsync()
        {
            //var filePath = @"Resources\DataEngineering\IncomingFileToMatch.csv";
            var filePath = @"Resources\DataEngineering\1000AddressToMatch.csv";
            byte[] fileBytes = File.ReadAllBytes(filePath);
            FileInfo fi = new FileInfo(filePath);
            var fileName = fi.Name;

            // When
            //await addressClient.MatchPatientAddressDataAsync(fileBytes, fileName);

            // Then

        }
    }
}
