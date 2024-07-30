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

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressTests
    {
        [Fact(Skip = "Will fix in another PR.")]
        public async Task ProcessResolvedAddressDataAsync()
        [Fact]
        public async Task ShouldLoadAddressesToResolveAsync()
        {
            // Given
            //string addressContainer = this.blobContainers.Addresses;
            string inputFilename = "ShouldLoadAddressesToResolveSetup.csv";
            string assembly = Assembly.GetExecutingAssembly().Location;
            string projectRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(assembly), @"..\..\.."));
            Guid expectedUniqueRef = Guid.Parse("7b41335a-f2cf-4949-8b83-c5b210446631");

            string inputFilePath = Path.Combine(
                projectRoot,
                $@"Resources/Clients/Address/{inputFilename}");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            Stream inputStream = new MemoryStream(inputData);

            // When
            //Guid? returnedBatchGuid = await addressClient.ProcessResolvedAddressDataAsync();
            await this.addressClient.LoadAddressesToResolveAsync(inputStream, inputFilename);

            //// Then
            //Assert.True(returnedBatchGuid == null || returnedBatchGuid != Guid.Empty,
            //   "The returned GUID should be either null or a valid GUID.");

            //if (returnedBatchGuid != null)
            //{
            //    string fileName = $"{returnedBatchGuid.ToString()}.csv";

            //    Document uploadedDocument =
            //        await this.documentService.RetrieveDocumentByFileNameAsync(fileName, addressContainer);

            //    Assert.NotNull(uploadedDocument);
            //    await this.documentService.RemoveDocumentByFileNameAsync(fileName, addressContainer);

            //    Document uploadedDocumentDeleteCheck =
            //       await this.documentService.RetrieveDocumentByFileNameAsync(fileName, addressContainer);

            //    Assert.Null(uploadedDocumentDeleteCheck);
            //}
            // Then
            ResolvedAddress retrievedAddress = this.resolvedAddressService.RetrieveAllResolvedAddresses().
                Where(resolvedAddress => resolvedAddress.UniqueReference == expectedUniqueRef).FirstOrDefault();
        }
    }
}