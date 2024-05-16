// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses(dateTimeOffset);

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                await this.resolvedAddressProcessingService.AddResolvedAddressAsync(resolvedAddress);
            }

            // When
            Guid actualBatchReference =
                await this.addressClient.ProcessResolvedAddressDataAsync();

            // Then
            string fileName = $"{actualBatchReference.ToString()}.csv";
            await this.documentService.RemoveDocumentByFileNameAsync(fileName, addressContainer);

            foreach (var resolvedAddress in randomResolvedAddresses)
            {
                await this.resolvedAddressProcessingService.RemoveResolvedAddressByIdAsync(resolvedAddress.Id);
            }
        }
    }
}
