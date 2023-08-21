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
using System.Web;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis
{
    public partial class DecryptionsApiTests
    {
        [Fact]
        public async Task ShouldDecryptFileAsync()
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
            byte[] encryptedData = await this.apiBroker.cryptographyProvider.EncryptAsync(documentData);

            Document document = new Document
            {
                DocumentData = encryptedData,
                FileName = inputFileName
            };

            await this.apiBroker.documentService.AddDocumentAsync(document);

            //When
            await this.apiBroker.GetDocumentByFileNameToDecryptAsync(HttpUtility.UrlEncode(inputIngestionTracking.FileName));

            //Then
            IngestionTracking decryptedIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(expectedIngestionTracking.Id);

            decryptedIngestionTracking.Decrypted.Should().BeTrue();

            await DeleteAuditRecordsAsync(randomIngestionTracking);
            await this.apiBroker.documentService.RemoveDocumentByFileNameAsync(document.FileName);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }
    }
}
