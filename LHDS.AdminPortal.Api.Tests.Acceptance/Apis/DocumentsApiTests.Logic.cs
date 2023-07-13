// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
//using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Documents;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Foundations.Documents;
using Microsoft.AspNetCore.Mvc;
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
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
            string randomFileName = GetRandomString();
            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = randomFileName
            };

            await this.apiBroker.documentService.AddDocumentAsync(document);

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
               supplierId: randomSupplier.Id);

            ingestionTracking.DecryptedFileName = document.FileName;

            // when
            ActionResult<Document> documentResult = await this.apiBroker.GetDownloadLinkAsync(randomFileName);

            // then
            documentResult.Result.Should().BeOfType<OkObjectResult>();

            IngestionTracking retrievedIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(ingestionTracking.Id);

            Document returnedDocument = ((OkObjectResult)documentResult.Result).Value as Document;

            returnedDocument.FileName.Should().Be(document.FileName);
            returnedDocument.DocumentData.Should().BeEquivalentTo(document.DocumentData);

            // clean up
            await this.apiBroker.documentService.RemoveDocumentByFileNameAsync(document.FileName);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }
    }
}