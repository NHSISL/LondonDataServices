// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact(Skip = "Hassan to fix as part of his appcetance tests for UPRN")]
        public async Task ShouldUploadResolvedAddressesAsync()
        {
            await ValueTask.CompletedTask;
            //// Given
            //DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            //string addressContainer = this.blobContainers.Addresses;

            //List<ResolvedAddress> randomMatchedResolvedAddresses =
            //    CreateRandomResolvedAddresses(dateTimeOffset, true);

            //List<ResolvedAddress> randomUnMatchedResolvedAddresses =
            //    CreateRandomResolvedAddresses(dateTimeOffset, false);

            //List<ResolvedAddress> randomResolvedAddresses = new List<ResolvedAddress>();
            //randomResolvedAddresses.AddRange(randomMatchedResolvedAddresses);
            //randomResolvedAddresses.AddRange(randomUnMatchedResolvedAddresses);

            //foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            //{
            //    await this.resolvedAddressProcessingService.AddResolvedAddressAsync(resolvedAddress);
            //}

            //Stream outputStream = new MemoryStream();

            //// When
            //Guid? actualBatchReference =
            //    await this.addressClient.ProcessResolvedAddressDataAsync();

            //// Then
            //string fileName = $"{actualBatchReference.ToString()}.csv";
            //await this.documentService.RetrieveDocumentByFileNameAsync(output: outputStream, fileName, addressContainer);
            //byte[] documentData = ReadAllBytesFromStream(outputStream);
            //string uploadedData = Encoding.ASCII.GetString(documentData);

            //foreach (var resolvedAddress in randomMatchedResolvedAddresses)
            //{
            //    uploadedData.Should().Contain(resolvedAddress.UniqueReference.ToString());

            //    ResolvedAddress matchedResolvedAddress =
            //        await this.resolvedAddressProcessingService.RetrieveResolvedAddressByIdAsync(resolvedAddress.Id);

            //    matchedResolvedAddress.IsProcessed = true;
            //}

            //foreach (var resolvedAddress in randomUnMatchedResolvedAddresses)
            //{
            //    uploadedData.Should().NotContain(resolvedAddress.UniqueReference.ToString());

            //    ResolvedAddress matchedResolvedAddress =
            //        await this.resolvedAddressProcessingService.RetrieveResolvedAddressByIdAsync(resolvedAddress.Id);

            //    matchedResolvedAddress.IsProcessed = true;
            //}

            //foreach (var resolvedAddress in randomUnMatchedResolvedAddresses)
            //{
            //    ResolvedAddress unMatchedResolvedAddress =
            //        await this.resolvedAddressProcessingService.RetrieveResolvedAddressByIdAsync(resolvedAddress.Id);

            //    unMatchedResolvedAddress.IsProcessed = false;
            //}

            //await this.documentService.RemoveDocumentByFileNameAsync(fileName, addressContainer);

            //foreach (var resolvedAddress in randomResolvedAddresses)
            //{
            //    await this.resolvedAddressProcessingService.RemoveResolvedAddressByIdAsync(resolvedAddress.Id);
            //}
        }
    }
}
