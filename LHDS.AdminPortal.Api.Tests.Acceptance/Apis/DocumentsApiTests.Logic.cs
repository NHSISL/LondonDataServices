// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Foundations.Documents;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Documents
{
    public partial class DocumentsApiTests
    {
        [Fact]
        public async Task ShouldGetDownloadLinkAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            string encryptedFilePath = "encrypted";
            string decryptedFilePath = "inbox/landing";
            string container = "emislanding";

            IngestionTracking randomIngestionTracking =
                await PostRandomIngestionTrackingAsync(randomSupplier.Id, encryptedFilePath, decryptedFilePath);

            string inputFileName = randomIngestionTracking.DecryptedFileName;
            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = inputFileName
            };

            Document expectedDocument = document;

            await this.apiBroker.documentService.AddDocumentAsync(document, container);

            // when
            Document actualDocument = await this.apiBroker.GetDownloadLinkAsync(WebUtility.UrlEncode(inputFileName));

            // then
            actualDocument.Should().BeEquivalentTo(expectedDocument);

            IngestionTracking retrievedIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(randomIngestionTracking.Id);

            actualDocument.FileName.Should().Be(expectedDocument.FileName);
            actualDocument.DocumentData.Should().BeEquivalentTo(expectedDocument.DocumentData);

            // clean up
            await this.apiBroker.documentService.RemoveDocumentByFileNameAsync(document.FileName, container);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }
    }
}