// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldLoadAddressDataAsync()
        {
            // Given
            string assembly = Assembly.GetExecutingAssembly().Location;
            char separator = Path.DirectorySeparatorChar;

            string projectRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(assembly),
                $"..{separator}..{separator}.."));

            string inputFilePath = Path.Combine(
                projectRoot,

                @"Resources/Clients/Address/CombinedFiles_London5k.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            Stream inputStream = new MemoryStream(inputData);

            // When
            await this.addressClient.LoadAddressDataAsync(inputStream, "CombinedFiles_London5k.zip");
        }
    }
}
