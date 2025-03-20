// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldLoadAddressesToResolveAsync()
        {
            // Given
            string assembly = Assembly.GetExecutingAssembly().Location;
            string projectRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(assembly), @"..\..\.."));

            string inputFilePath = Path.Combine(
                projectRoot,
                @"Resources/Clients/Address/registrar-request-test.csv");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            Stream inputStream = new MemoryStream(inputData);

            // When
            await this.addressClient.LoadAddressesToResolveAsync(inputStream, "registrar-request-test.csv");

            // Then
        }
    }
}