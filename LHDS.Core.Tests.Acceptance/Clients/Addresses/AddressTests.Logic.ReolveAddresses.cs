// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldLoadAddressesToResolveAsync()
        {
            // Given
            string inputFilename = GetRandomString();
            string assembly = Assembly.GetExecutingAssembly().Location;
            string projectRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(assembly), @"..\..\.."));
            Guid expectedUniqueRef = Guid.Parse("7b41335a-f2cf-4949-8b83-c5b210446631");

            string inputFilePath = Path.Combine(
                projectRoot,
                @"Resource/Clients/Address/ShouldUploadAddressesSetup.csv");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            Stream inputStream = new MemoryStream(inputData);

            // When
            await this.addressClient.LoadAddressesToResolveAsync(inputStream, inputFilename);

            // Then
            ResolvedAddress retrievedAddress = this.resolvedAddressService.RetrieveAllResolvedAddresses().
                Where(resolvedAddress => resolvedAddress.UniqueReference == expectedUniqueRef).FirstOrDefault();

            await this.resolvedAddressService.RemoveResolvedAddressByIdAsync(retrievedAddress.Id);
        }
    }
}

