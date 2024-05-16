// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldUploadResolvedAddressesAsync()
        {
            // Given
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string addressContainer = this.blobContainers.Addresses;

            List<ResolvedAddress> randomMatchedResolvedAddresses =
                CreateRandomResolvedAddresses(dateTimeOffset, true);

            List<ResolvedAddress> randomUnMatchedResolvedAddresses =
                CreateRandomResolvedAddresses(dateTimeOffset, false);

            List<ResolvedAddress> randomResolvedAddresses = new List<ResolvedAddress>();
            randomResolvedAddresses.AddRange(randomMatchedResolvedAddresses);
            randomResolvedAddresses.AddRange(randomUnMatchedResolvedAddresses);

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                await this.resolvedAddressProcessingService.AddResolvedAddressAsync(resolvedAddress);
            }

            // When
            Guid actualBatchReference =
                await this.addressClient.ProcessResolvedAddressDataAsync();

            // Then
            string fileName = $"{actualBatchReference.ToString()}.csv";
            //await this.documentService.RemoveDocumentByFileNameAsync(fileName, addressContainer);

            foreach (var resolvedAddress in randomMatchedResolvedAddresses)
            {
                ResolvedAddress matchedResolvedAddress = 
                    await this.resolvedAddressProcessingService.RetrieveResolvedAddressByIdAsync(resolvedAddress.Id);

                matchedResolvedAddress.IsProcessed = true;
            }

            foreach (var resolvedAddress in randomUnMatchedResolvedAddresses)
            {
                ResolvedAddress unMatchedResolvedAddress =
                    await this.resolvedAddressProcessingService.RetrieveResolvedAddressByIdAsync(resolvedAddress.Id);

                unMatchedResolvedAddress.IsProcessed = false;
            }

            foreach (var resolvedAddress in randomResolvedAddresses)
            {
                await this.resolvedAddressProcessingService.RemoveResolvedAddressByIdAsync(resolvedAddress.Id);
            }
        }
    }
}
