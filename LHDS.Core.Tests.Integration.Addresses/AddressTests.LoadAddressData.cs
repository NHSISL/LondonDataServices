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
        public async Task ShouldLoadAddressDataAsync()
        {
            // Given
            string assembly = Assembly.GetExecutingAssembly().Location;
            char separator = Path.DirectorySeparatorChar;

            string projectRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(assembly),
                $"..{separator}..{separator}.."));

            string inputFilePath = Path.Combine(
                projectRoot,

                @"Resource/Clients/Address/Ordinance_data_London.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            Stream inputStream = new MemoryStream(inputData);

            // When
            await this.addressClient.LoadAddressDataAsync(inputStream, "Ordinance_data_London.zip");
        }
    }
}
