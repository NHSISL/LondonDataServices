// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
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
            try
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
            catch (Exception ex)
            {
                output.WriteLine($"Error: {ex.Message}{Environment.NewLine}" +
                    $"{ex.InnerException.Message}{Environment.NewLine}" +
                    $"{ex.InnerException.StackTrace}");

                throw ex.InnerException;
            }
        }

        [Fact]
        public async Task ShouldLandDocumentByFileNameForNewIngestionTrackingAsync()
        {
            try
            {
                //Given
                List<Document> retrievedDocuments =
                    await this.apiBroker.downloadService.RetrieveListOfDocumentsToProcessAsync();

                byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());
                byte[] encryptedData = await this.apiBroker.cryptographyProvider.EncryptAsync(documentData);
                Document retrievedDocument = retrievedDocuments[1];
                retrievedDocument.DocumentData = encryptedData;
                string decryptedFilePath = decryptedFolder;
                await CleanupTask(retrievedDocument.FileName);
                bool hasExisitingSupplier = (await this.apiBroker.FindSupplierByIdAsync(supplierId)).Any();
                bool hasExisitingDataSet = (await this.apiBroker.FindDataSetByIdAsync(dataSetId)).Any();

                bool hasExistingDataSetSpecification =
                    (await this.apiBroker.FindtDataSetSpecificationByIdAsync(dataSetSpecificationId)).Any();

                if (!hasExisitingSupplier)
                {
                    await PostLandingSupplierAsync(supplierId);
                }

                if (!hasExisitingDataSet)
                {
                    await PostDataSetAsync(supplierId, dataSetId);
                }

                if (!hasExistingDataSetSpecification)
                {
                    await PostDataSetSpecificationAsync(dataSetSpecificationId, dataSetId);
                }


                DataSet activeDataSet = await this.apiBroker.GetDataSetByIdAsync(dataSetId);

                DataSetSpecification activeDataSetSpecification =
                    await this.apiBroker.GetDataSetSpecificationByIdAsync(dataSetSpecificationId);

                string expectedDecryptedFileName =
                    $"/{decryptedFilePath}" +
                    $"/{activeDataSet.DataSetName}" +
                    $"/{activeDataSetSpecification.Id}" +
                    $"/{retrievedDocument.FileName.Split('_')[3]}" +
                    $"{retrievedDocument.FileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

                //When
                string actualDecryptedFileName =
                    await this.apiBroker.GetLandingDocumentByFileNameAsync(retrievedDocument.FileName);

                //Then 
                actualDecryptedFileName.Should().BeEquivalentTo(expectedDecryptedFileName);
                await CleanupTask(retrievedDocument.FileName);
            }
            catch (Exception ex)
            {
                output.WriteLine($"Error: {ex.Message}{Environment.NewLine}" +
                    $"{ex.InnerException.Message}{Environment.NewLine}" +
                    $"{ex.InnerException.StackTrace}");

                throw ex.InnerException;
            }

        }

        private async ValueTask CleanupTask(string fileName)
        {
            var maybeIngestionTracking = await this.apiBroker.ingestionTrackingService
                .RetrieveIngestionTrackingByFileNameAsync(fileName);

            if (maybeIngestionTracking != null)
            {
                await RemoveAuditRecords(maybeIngestionTracking);

                await this.apiBroker.ingestionTrackingService
                    .RemoveIngestionTrackingByIdAsync(maybeIngestionTracking.Id);
            }
        }

        private async ValueTask CleanupTask(Guid ingestionTrackingId)
        {
            var maybeIngestionTracking = await this.apiBroker.ingestionTrackingService
                .RetrieveIngestionTrackingByIdAsync(ingestionTrackingId);

            if (maybeIngestionTracking != null)
            {
                await RemoveAuditRecords(maybeIngestionTracking);

                await this.apiBroker.ingestionTrackingService
                    .RemoveIngestionTrackingByIdAsync(ingestionTrackingId);
            }
        }

        private async Task RemoveAuditRecords(
            Core.Models.Foundations.IngestionTrackings.IngestionTracking ingestionTracking)
        {
            var audits = this.apiBroker.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.apiBroker.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }
        }
    }
}