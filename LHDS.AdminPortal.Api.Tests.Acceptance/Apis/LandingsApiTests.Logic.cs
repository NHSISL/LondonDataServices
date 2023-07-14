// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Foundations.Documents;
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
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = randomIngestionTracking.EncryptedFileName
            };

            await this.apiBroker.documentService.AddDocumentAsync(document);

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                supplierId: randomSupplier.Id);

            ingestionTracking.DecryptedFileName = document.FileName;

            // when
            await this.apiBroker.GetLandingDocumentByFileNameAsync(randomIngestionTracking.EncryptedFileName);

            // then
            IngestionTracking createdIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(ingestionTracking.Id);

            createdIngestionTracking.FileName = document.FileName;

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }
    }
}