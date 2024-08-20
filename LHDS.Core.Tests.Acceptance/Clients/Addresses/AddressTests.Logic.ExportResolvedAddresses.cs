// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldExportResolvedAddressesAsync()
        {
            ////Given
            //List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(GetRandomNumber());

            //foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            //{
            //    await this.resolvedAddressService.AddResolvedAddressAsync(resolvedAddress);
            //}

            //string expectedAddresses = await MapObjectToCsv(randomResolvedAddresses);

            //When
            await this.addressClient.ExportResolvedAddressesAsync();

            //Then
            //ResolvedAddress retrievedResolvedAddress =
            //    await this.resolvedAddressService.RetrieveResolvedAddressByIdAsync(randomResolvedAddresses[0].Id);

            //Guid? batchReference = retrievedResolvedAddress.BatchReference;

            //MemoryStream retrievedDocumentStream = new MemoryStream();
            //string csvFileName = $"out/{batchReference}.csv";

            //await this.documentService.RetrieveDocumentByFileNameAsync(
            //    retrievedDocumentStream, csvFileName, blobContainers.Addresses);

            //retrievedDocumentStream.Position = 0;
            //string retrievedDocumentCsv;

            //using (StreamReader reader = new StreamReader(retrievedDocumentStream))
            //{
            //    retrievedDocumentCsv = await reader.ReadToEndAsync();
            //}

            //List<ResolvedAddress> retrievedCsvAddresses = await this.MapCsvToObject(data: retrievedDocumentCsv);
            //randomResolvedAddresses.Count.Should().Be(randomResolvedAddresses.Count);

            //foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            //{
            //    ResolvedAddress address = retrievedCsvAddresses.FirstOrDefault(
            //        retrievedAddress => retrievedAddress.UniqueReference == resolvedAddress.UniqueReference);

            //    address.Should().NotBeNull();
            //    await this.resolvedAddressService.RemoveResolvedAddressByIdAsync(resolvedAddress.Id);
            //}

            //await this.documentService.RemoveDocumentByFileNameAsync(csvFileName, blobContainers.Addresses);
        }
    }
}