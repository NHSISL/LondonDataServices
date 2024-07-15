//// ---------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------

//using System;
//using System.Threading.Tasks;
//using LHDS.Core.Models.Foundations.Documents;
//using Xunit;

//namespace LHDS.Core.Tests.Integration.Addresses
//{
//    public partial class AddressTests
//    {
//        [Fact]
//        public async Task ProcessResolvedAddressDataAsync()
//        {
//            // Given
//            string addressContainer = this.blobContainers.Addresses;

//            // When
//            Guid? returnedBatchGuid = await addressClient.ProcessResolvedAddressDataAsync();

//            // Then
//            Assert.True(returnedBatchGuid == null || returnedBatchGuid != Guid.Empty,
//               "The returned GUID should be either null or a valid GUID.");

//            if (returnedBatchGuid != null)
//            {
//                string fileName = $"{returnedBatchGuid.ToString()}.csv";

//                Document uploadedDocument =
//                    await this.documentService.RetrieveDocumentByFileNameAsync(fileName, addressContainer);

//                Assert.NotNull(uploadedDocument);
//                await this.documentService.RemoveDocumentByFileNameAsync(fileName, addressContainer);

//                Document uploadedDocumentDeleteCheck =
//                   await this.documentService.RetrieveDocumentByFileNameAsync(fileName, addressContainer);

//                Assert.Null(uploadedDocumentDeleteCheck);
//            }
//        }
//    }
//}