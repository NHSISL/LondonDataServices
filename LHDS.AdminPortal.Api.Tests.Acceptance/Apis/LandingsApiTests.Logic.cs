// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Foundations.Documents;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Landings
{
    public partial class LandingsApiTests
    {
        [Fact]
        public async Task ShouldLandDocumentByFileNameForExistingIngestionTrackingAsync()
        {
            //Given
            List<Document> retrievedDocuments =
                await this.apiBroker.downloadService.RetrieveListOfDocumentsToProcessAsync();

            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());
            byte[] encryptedData = await this.apiBroker.cryptographyProvider.EncryptAsync(documentData);
            Document retrievedDocument = retrievedDocuments[0];
            retrievedDocument.DocumentData = encryptedData;
            Supplier randomSupplier = await PostRandomSupplierAsync();
            string encryptedFilePath = encryptedFolder;
            string decryptedFilePath = decryptedFolder;
            await CleanupTask(retrievedDocument.FileName);

            IngestionTracking randomIngestionTracking =
                await PostRandomIngestionTrackingAsync(
                    randomSupplier.Id,
                    retrievedDocument.FileName,
                    encryptedFilePath,
                    decryptedFilePath);

            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedIngestionTracking = inputIngestionTracking;

            //When
            string actualDecryptedFileName =
                await this.apiBroker.GetLandingDocumentByFileNameAsync(retrievedDocument.FileName);

            //Then
            actualDecryptedFileName.Should().BeEquivalentTo(expectedIngestionTracking.DecryptedFileName);
            await CleanupTask(expectedIngestionTracking.Id);
        }

        private async ValueTask CleanupTask(string fileName)
        {
            var maybeIngestionTracking = await this.apiBroker.ingestionTrackingService
                .RetrieveIngestionTrackingByFileNameAsync(fileName);

            if (maybeIngestionTracking != null)
            {
                var audits = this.apiBroker.auditService.RetrieveAllAudits()
                .Where(audit => audit.IngestionTrackingId == maybeIngestionTracking.Id);

                foreach (var audit in audits)
                {
                    await this.apiBroker.auditService.RemoveAuditByIdAsync(audit.Id);
                }

                await this.apiBroker.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(maybeIngestionTracking.Id);
            }
        }

        private async ValueTask CleanupTask(Guid ingestionTrackingId)
        {
            var maybeIngestionTracking = await this.apiBroker.ingestionTrackingService
                .RetrieveIngestionTrackingByIdAsync(ingestionTrackingId);

            if (maybeIngestionTracking != null)
            {
                var audits = this.apiBroker.auditService.RetrieveAllAudits()
                    .Where(audit => audit.IngestionTrackingId == ingestionTrackingId);

                foreach (var audit in audits)
                {
                    await this.apiBroker.auditService.RemoveAuditByIdAsync(audit.Id);
                }

                await this.apiBroker.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTrackingId);
            }
        }

        [Fact]
        public async Task ShouldLandDocumentByFileNameForNewIngestionTrackingAsync()
        {
            //Given
            List<Document> retrievedDocuments =
                await this.apiBroker.downloadService.RetrieveListOfDocumentsToProcessAsync();

            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());
            byte[] encryptedData = await this.apiBroker.cryptographyProvider.EncryptAsync(documentData);
            Document retrievedDocument = retrievedDocuments[1];
            retrievedDocument.DocumentData = encryptedData;
            string decryptedFilePath = decryptedFolder;
            Guid landingSupplierId = supplierId;
            await CleanupTask(retrievedDocument.FileName);
            await this.apiBroker.DeleteSupplierByIdAsync(landingSupplierId);
            await PostLandingSupplierAsync(landingSupplierId);
            string expectedDecryptedFileName = 
                $"/{decryptedFilePath}" +
                $"{retrievedDocument.FileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

            //When
            string actualDecryptedFileName =
                await this.apiBroker.GetLandingDocumentByFileNameAsync(retrievedDocument.FileName);

            //Then 
            actualDecryptedFileName.Should().BeEquivalentTo(expectedDecryptedFileName);
            await CleanupTask(retrievedDocument.FileName);
        }
    }
}