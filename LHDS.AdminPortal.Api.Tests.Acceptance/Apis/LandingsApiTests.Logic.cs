// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Foundations.Documents;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Audits
{
    public partial class LandingsApiTests
    {
        [Fact]
        public async Task ShouldGetLandingDocumentByFileNameAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            string encryptedFilePath = "encrypted";
            string decryptedFilePath = "decrypted";

            IngestionTracking randomIngestionTracking =
                await PostRandomIngestionTrackingAsync(randomSupplier.Id, encryptedFilePath, decryptedFilePath);

            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedIngestionTracking = inputIngestionTracking;

            string inputFileName = randomIngestionTracking.EncryptedFileName;
            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = inputFileName
            };

            await this.apiBroker.documentService.AddDocumentAsync(document);

            // when
            ActionResult<IngestionTracking> result = 
                await this.apiBroker.GetLandingDocumentByFileNameAsync(randomIngestionTracking.EncryptedFileName);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            IngestionTracking createdIngestionTracking = 
                await this.apiBroker.GetIngestionTrackingByIdAsync(randomIngestionTracking.Id);

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }
    }
}