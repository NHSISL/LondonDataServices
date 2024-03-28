// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Foundations.Documents;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Landings
{
    public partial class EmisLandingsApiTests
    {
        [Fact]
        public async Task ShouldLandDocumentByFileNameForExistingIngestionTrackingAsync()
        {
            //Given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            await this.apiBroker.PostSubscriberCredentialAsync(inputSubscriberCredential);
            string randomFileName = GetRandomFileName(inputSubscriberCredential.Id);
            string randomFilePath = CreateRandomFilePath(inputSubscriberCredential.Id, randomFileName);
            Supplier randomSupplier = await PostRandomSupplierAsync();
            string encryptedFilePath = encryptedFolder;
            string decryptedFilePath = decryptedFolder;

            Document document = new Document
            {
                FileName = randomFileName,
                DocumentData = Encoding.ASCII.GetBytes(GetRandomString()),
            };

            await this.apiBroker.documentService.AddDocumentAsync(document, "emislanding");

            IngestionTracking randomIngestionTracking =
                await PostRandomIngestionTrackingAsync(
                    randomSupplier.Id,
                    randomFileName,
                    encryptedFilePath,
                    decryptedFilePath);

            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedIngestionTracking = inputIngestionTracking;

            //When
            string actualDecryptedFileName =
                await this.apiBroker.ReLandDocumentByFileNameAsync(fileName);

            //Then
            actualDecryptedFileName.Should().BeEquivalentTo(expectedIngestionTracking.DecryptedFileName);
            await CleanupTask(expectedIngestionTracking.Id);
            await this.apiBroker.documentService.RemoveDocumentByFileNameAsync(fileName, "emislanding");
        }

        [Fact]
        public async Task ShouldRedecryptDocumentByIngestionTrackingAsync()
        {
            //given 
            Supplier randomSupplier = await PostRandomSupplierAsync();
            string fileName = GetRandomString(10);
            string encryptedFilePath = "encrypted";
            string decryptedFilePath = "decrypted";

            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(
                    supplierId: randomSupplier.Id,
                    fileName,
                    encryptedFilePath,
                    decryptedFilePath);

            randomIngestionTracking.Decrypted = true;
            await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            // when
            await this.apiBroker.RedecryptDocumentByIngestionTrackingIdAsync(randomIngestionTracking.Id);

            // then
            IngestionTracking redecryptedIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(randomIngestionTracking.Id);

            redecryptedIngestionTracking.Decrypted.Should().BeFalse();
            await CleanupTask(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        private async ValueTask CleanupTask(string fileName)
        {
            IngestionTracking? maybeIngestionTracking =
                await this.apiBroker.FindIngestionTrackingByFileNameAsync(fileName);

            if (maybeIngestionTracking == null)
            {
                return;
            }

            var ingestionTrackingAudits = await this.apiBroker
                .FindIngestionTrackingAuditByIngestionTrackingIdAsync(maybeIngestionTracking.Id);

            foreach (var ingestionTrackingAudit in ingestionTrackingAudits)
            {
                await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(ingestionTrackingAudit.Id);
            }

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(maybeIngestionTracking.Id);
        }

        private async ValueTask CleanupTask(Guid ingestionTrackingId)
        {
            var maybeIngestionTracking = await this.apiBroker.GetIngestionTrackingByIdAsync(ingestionTrackingId);

            if (maybeIngestionTracking == null)
            {
                return;
            }

            var ingestionTrackingAudits = await this.apiBroker
                .FindIngestionTrackingAuditByIngestionTrackingIdAsync(maybeIngestionTracking.Id);

            foreach (var ingestionTrackingAudit in ingestionTrackingAudits)
            {
                await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(ingestionTrackingAudit.Id);
            }

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(maybeIngestionTracking.Id);
        }
    }
}