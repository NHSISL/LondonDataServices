// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Decryptions
{
    public partial class DecryptionTests
    {
        [Fact]
        public async Task ShouldDecryptNewDocumentsAsync()
        {
            // Given
            string fileName = $"{GetRandomString()}.gpg";
            string encryptedFileName = $"/{landingConfiguration.EncryptedFolder}/{GetRandomString()}.gpg";
            string decryptedFileName = $"/{landingConfiguration.DecryptedFolder}/{GetRandomString()}";
            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());
            byte[] encryptedData = await this.cryptographyProvider.EncryptAsync(documentData);

            Document document = new Document
            {
                DocumentData = encryptedData,
                FileName = encryptedFileName
            };

            await this.documentService.AddDocumentAsync(document);

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                fileName,
                encryptedFileName,
                decryptedFileName,
                supplierId: this.landingConfiguration.LandingSupplierId);

            ingestionTracking = await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

            // When
            var actualString = await this.decryptionClient.DecryptAsync(fileName);

            // Then
            actualString.Should().BeEquivalentTo(ingestionTracking.DecryptedFileName);

            await DeleteAudits(ingestionTracking);
            await this.documentService.RemoveDocumentByFileNameAsync(ingestionTracking.DecryptedFileName);
            await this.documentService.RemoveDocumentByFileNameAsync(ingestionTracking.EncryptedFileName);
            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
        }

        private async Task DeleteAudits(IngestionTracking ingestionTracking)
        {
            var auditIds = this.auditService.RetrieveAllAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id)
                .Select(ingestionTracking => ingestionTracking.Id)
                .ToList();

            foreach (var id in auditIds)
            {
                await this.auditService.RemoveAuditByIdAsync(id);
            }

            if (this.auditService.RetrieveAllAudits()
                .Any(audit => audit.IngestionTrackingId == ingestionTracking.Id))
            {
                await DeleteAudits(ingestionTracking);
            }
        }
    }
}
