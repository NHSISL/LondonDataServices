// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
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
        public async Task ShouldGetExistingLandingDocumentByFileNameAsync()
        {
            // given
            List<Document> retrievedDocuments =
                await this.apiBroker.downloadService.RetrieveListOfDocumentsToProcessAsync();

            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());
            byte[] encryptedData = await this.apiBroker.cryptographyProvider.EncryptAsync(documentData);
            Document retrievedDocument = retrievedDocuments[0];
            retrievedDocument.DocumentData = encryptedData;

            // create ingestion tracking

            Supplier randomSupplier = await PostRandomSupplierAsync();
            string encryptedFilePath = "encrypted";
            string decryptedFilePath = "decrypted";

            IngestionTracking randomIngestionTracking =
                await PostRandomIngestionTrackingAsync(
                    randomSupplier.Id,
                    retrievedDocument.FileName,
                    encryptedFilePath,
                    decryptedFilePath);

            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedIngestionTracking = inputIngestionTracking;

            // when
            string actualDecryptedFileName =
                await this.apiBroker.GetLandingDocumentByFileNameAsync(retrievedDocument.FileName);

            List<Audit> retrievedAudits = await this.apiBroker.GetAllAuditsAsync();

            List<Audit> ingestionTrackingAudits =
                retrievedAudits.Where(audit => audit.IngestionTrackingId == inputIngestionTracking.Id).ToList();

            //Then
            actualDecryptedFileName.Should().BeEquivalentTo(expectedIngestionTracking.DecryptedFileName);

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(inputIngestionTracking.Id);

            foreach (var audit in ingestionTrackingAudits)
            {
                await this.apiBroker.DeleteAuditByIdAsync(audit.Id);
            }
        }

        [Fact]
        public async Task ShouldGetLandingDocumentNotOnFTPByFileNameAsync()
        {
            // given
            List<Document> retrievedDocuments =
                await this.apiBroker.downloadService.RetrieveListOfDocumentsToProcessAsync();

            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());
            byte[] encryptedData = await this.apiBroker.cryptographyProvider.EncryptAsync(documentData);
            Document retrievedDocument = retrievedDocuments[0];
            retrievedDocument.DocumentData = encryptedData;
            //var retrievedFileName = retrievedDocument.FileName.Split("/".ToCharArray());
            //retrievedDocument.FileName = retrievedFileName[retrievedFileName.Length - 1];

            // when
            ActionResult<string> result =
                await this.apiBroker.GetLandingDocumentByFileNameAsync(retrievedDocument.FileName);

            List<IngestionTracking> retrievedIngestionTrackings = await this.apiBroker.GetAllIngestionTrackingsAsync();

            IngestionTracking landingIngestionTracking =
                retrievedIngestionTrackings.FirstOrDefault(it => it.FileName == retrievedDocument.FileName);

            List<Audit> retrievedAudits = await this.apiBroker.GetAllAuditsAsync();

            List<Audit> ingestionTrackingAudits =
                retrievedAudits.Where(audit => audit.IngestionTrackingId == landingIngestionTracking.Id).ToList();

            //Then
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(landingIngestionTracking.Id);

            foreach (var audit in ingestionTrackingAudits)
            {
                await this.apiBroker.DeleteAuditByIdAsync(audit.Id);
            }
        }
    }
}