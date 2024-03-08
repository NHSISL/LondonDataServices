// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Documents;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Downloads;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
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
                SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
                SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
                Document randomDocument = CreateRandomDocument();

                Download inputDownload = new Download
                {
                    SubscriberCredential = inputSubscriberCredential,
                    Document = new Document { FileName = randomDocument.FileName }
                };

                List<Download> downloads = await this.apiBroker.RetrieveListOfDocumentsToProcessAsync(inputDownload);
                Supplier randomSupplier = await PostRandomSupplierAsync();
                string encryptedFilePath = encryptedFolder;
                string decryptedFilePath = decryptedFolder;
                await CleanupTask(randomDocument.FileName);

                IngestionTracking randomIngestionTracking =
                    await PostRandomIngestionTrackingAsync(
                        randomSupplier.Id,
                        randomDocument.FileName,
                        encryptedFilePath,
                        decryptedFilePath);

                IngestionTracking inputIngestionTracking = randomIngestionTracking;
                IngestionTracking expectedIngestionTracking = inputIngestionTracking;

                //When
                string actualDecryptedFileName =
                    await this.apiBroker.GetLandingDocumentByFileNameAsync(randomDocument.FileName);

                //Then
                actualDecryptedFileName.Should().BeEquivalentTo(expectedIngestionTracking.DecryptedFileName);
                await CleanupTask(expectedIngestionTracking.Id);
            }
            catch (Exception ex)
            {
                output.WriteLine($"Error: {ex.Message}{Environment.NewLine}" +
                    $"{ex?.StackTrace}{Environment.NewLine}" +
                    $"{ex?.InnerException?.Message}{Environment.NewLine}" +
                    $"{ex?.InnerException?.StackTrace}");

                throw;
            }
        }

        [Fact]
        public async Task ShouldLandDocumentByFileNameForNewIngestionTrackingAsync()
        {
            try
            {
                //Given
                SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
                SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
                Document randomDocument = CreateRandomDocument();

                Download inputDownload = new Download
                {
                    SubscriberCredential = inputSubscriberCredential,
                    Document = new Document { FileName = randomDocument.FileName }
                };

                List<Download> retrievedDownloads = await this.apiBroker.RetrieveListOfDocumentsToProcessAsync(inputDownload);
                await CleanupTask(randomDocument.FileName);
                bool hasExisitingSupplier = (await this.apiBroker.FindSupplierByIdAsync(supplierId)).Any();
                bool hasExisitingDataSet = (await this.apiBroker.FindDataSetByIdAsync(dataSetId)).Any();
                string decryptedFilePath = decryptedFolder;

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
                    $"/{randomDocument.FileName.Split('_')[3]}" +
                    $"{randomDocument.FileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

                //When
                string actualDecryptedFileName =
                    await this.apiBroker.GetLandingDocumentByFileNameAsync(randomDocument.FileName);

                //Then 
                actualDecryptedFileName.Should().BeEquivalentTo(expectedDecryptedFileName);
                await CleanupTask(randomDocument.FileName);
            }
            catch (Exception ex)
            {
                output.WriteLine($"Error: {ex.Message}{Environment.NewLine}" +
                    $"{ex?.StackTrace}{Environment.NewLine}" +
                    $"{ex?.InnerException?.Message}{Environment.NewLine}" +
                    $"{ex?.InnerException?.StackTrace}");

                throw;
            }
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