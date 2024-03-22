// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Decryptions
{
    public partial class DecryptionTests
    {
        [Fact]
        public async Task ShouldDecryptNewDocumentsAsync()
        {
            //Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();

            SubscriberCredential inputSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(
                    subscriberCredential: randomSubscriberCredential,
                    regenerateKeys: true,
                    externalUse: false);

            Guid supplierId = landingConfiguration.LandingSupplierId;
            DataSet activeDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);
            string randomFileName = GetRandomFileName(subscriberAgreementId: inputSubscriberCredential.Id);

            string randomFilePath = CreateRandomFilePath(
                subscriberAgreementId: inputSubscriberCredential.Id,
                fileName: randomFileName);


            string encryptedFileName = CreateRandomEncryptedFilePath(
                inputSubscriberCredential.Id,
                inputSubscriberCredential.SupplierSharingAgreementGuid);

            string decryptedFileName = CreateRandomDecryptedFilePath(
                activeDataSet.DataSetName,
                activeDataSetSpecification.Id,
                randomFilePath.Split('_')[2] + "_" + randomFilePath.Split('_')[3],
                inputSubscriberCredential.Id,
                inputSubscriberCredential.SupplierSharingAgreementGuid);

            string randomString = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);

            byte[] encryptedData = await this.cryptographyProvider.EncryptAsync(
                randomBytes,
                inputSubscriberCredential);

            Download fileToRetrieve = new Download
            {
                Document = new Document { FileName = randomFilePath },
                SubscriberCredential = inputSubscriberCredential
            };

            string encryptedFileSha256Hash =
                this.hashBroker.GenerateSha256Hash(fileToRetrieve.Document.DocumentData);

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                fileToRetrieve,
                encryptedFileName,
                decryptedFileName,
                encryptedFileSha256Hash,
                encryptedFileSize: fileToRetrieve.Document.DocumentData.Length,
                supplierId: this.landingConfiguration.LandingSupplierId);

            Document document = new Document
            {
                FileName = ingestionTracking.EncryptedFileName,
                DocumentData = encryptedData
            };

            await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);
            await this.documentProcessingService.AddDocumentAsync(document, blobContainers.EmisLanding);

            //When
            var actualString = await this.decryptionClient.DecryptAsync(ingestionTracking.EncryptedFileName);

            //Then
            actualString.Should().BeEquivalentTo(ingestionTracking.DecryptedFileName);

            ingestionTracking.Should().NotBeNull();

            IngestionTracking decryptedIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

            await this.subscriberCredentialOrchestration
                .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: inputSubscriberCredential.Id);

            await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                  fileName: ingestionTracking.EncryptedFileName,
                  container: blobContainers.EmisLanding);

            await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                  fileName: ingestionTracking.DecryptedFileName,
                  container: blobContainers.Versioner);

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
        }
    }
}
