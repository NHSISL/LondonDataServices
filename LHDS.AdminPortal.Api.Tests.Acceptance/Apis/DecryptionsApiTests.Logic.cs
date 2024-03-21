// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using LHDS.AdminPortal.Api.Models.Controllers.Documents;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Foundations.Documents;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis
{
    public partial class DecryptionsApiTests
    {
        [Fact]
        public async Task ShouldDecryptFileAsync()
        {
            // given
            Guid subscriberCredentialId = Guid.NewGuid();
            string encryptedFileContainer = "emislanding";
            string decryptedFileContainer = "versioner";
            Supplier randomSupplier = await PostRandomSupplierAsync();
            string encryptedFilePath = "encrypted";
            string decryptedFilePath = "decrypted";


            IngestionTracking randomIngestionTracking =
                await PostRandomIngestionTrackingAsync(randomSupplier.Id, encryptedFilePath, decryptedFilePath);

            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedIngestionTracking = inputIngestionTracking;
            string decryptedData = GetRandomString();
            byte[] decryptedDocumentData = Encoding.ASCII.GetBytes(decryptedFileContainer);
            string inputFileName = randomIngestionTracking.EncryptedFileName;
            this.cryptographyBroker.EncryptAsync(decryptedDocumentData, subscriberCredential)
            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());

            Document document = new Document
            {
                DocumentData = encryptedData,
                FileName = inputFileName
            };

            DocumentsModel documentsModel = new DocumentsModel
            {
                Document = document,
                Container = encryptedFileContainer
            };

            await this.apiBroker.PostDocumentAsync(documentsModel);

            //When
            await this.apiBroker.GetDocumentByFileNameToDecryptAsync(
                HttpUtility.UrlEncode(inputIngestionTracking.FileName));

            //Then
            IngestionTracking decryptedIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(expectedIngestionTracking.Id);

            decryptedIngestionTracking.Decrypted.Should().BeTrue();

            await DeleteAuditRecordsAsync(randomIngestionTracking);

            await this.apiBroker.DeleteDocumentByFileNameAsync(
                fileName: decryptedIngestionTracking.EncryptedFileName,
                container: encryptedFileContainer);

            await this.apiBroker.DeleteDocumentByFileNameAsync(
                fileName: decryptedIngestionTracking.DecryptedFileName,
                container: decryptedFileContainer);

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }
    }
}
