// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldExportResolvedAddressesAsync()
        {
            //Given

            //When
            List<Guid> batchRefIds = await this.addressClient.ExportResolvedAddressesAsync();

            //Then
            foreach (var id in batchRefIds)
            {
                MemoryStream retrievedDocumentStream = new MemoryStream();
                string csvFileName = $"out/{id}.csv";

                await this.documentService.RetrieveDocumentByFileNameAsync(
                    retrievedDocumentStream, csvFileName, blobContainers.Addresses);

                retrievedDocumentStream.Should().NotBeNull();
            }
        }
    }
}