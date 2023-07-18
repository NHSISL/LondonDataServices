// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using Xunit;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using System.Text;
using LHDS.Core.Models.Foundations.Documents;
using System.Diagnostics.SymbolStore;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis
{
    public partial class DecryptionsApiTests
    {
        [Fact]
        public async Task ShouldDecryptFileAsync()
        {
            //Given
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

            ingestionTracking.EncryptedFileName = document.FileName;
            await this.apiBroker.PostIngestionTrackingAsync(ingestionTracking);

            //When
            await this.apiBroker.DecryptFileAsync(document.FileName);

            //Then
            IngestionTracking decryptedIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(ingestionTracking.Id);

            decryptedIngestionTracking.Decrypted.Should().BeTrue();

            await DeleteAuditRecordsAsync(randomIngestionTracking);
            await this.apiBroker.documentService.RemoveDocumentByFileNameAsync(document.FileName);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }
    }
}
