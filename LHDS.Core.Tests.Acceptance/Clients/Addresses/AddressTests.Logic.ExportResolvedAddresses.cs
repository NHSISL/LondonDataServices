// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldExportResolvedAddressesAsync()
        {
            //Given
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(GetRandomNumber());
            Guid? batchReference = Guid.NewGuid();

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                await this.resolvedAddressService.AddResolvedAddressAsync(resolvedAddress);
            }

            //When
            await this.addressClient.ExportResolvedAddressesAsync();

            //Then
            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                ResolvedAddress retrievedResolvedAddress =
                    await this.resolvedAddressService.RetrieveResolvedAddressByIdAsync(resolvedAddress.Id);

                retrievedResolvedAddress.IsProcessing.Should().Be(false);
                retrievedResolvedAddress.RetryCount.Should().Be(0);
                retrievedResolvedAddress.IsExported.Should().Be(true);
                retrievedResolvedAddress.IsProcessed.Should().Be(true);

                await this.resolvedAddressService.RemoveResolvedAddressByIdAsync(resolvedAddress.Id);
                batchReference = retrievedResolvedAddress.BatchReference;
            }

            Stream retrievedDocumentStream = new MemoryStream();
            string csvFileName = $"out/{batchReference}.csv";

            await this.documentService.RetrieveDocumentByFileNameAsync(
                retrievedDocumentStream, csvFileName, blobContainers.Addresses);

            await this.documentService.RemoveDocumentByFileNameAsync(csvFileName, blobContainers.Addresses);

            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}