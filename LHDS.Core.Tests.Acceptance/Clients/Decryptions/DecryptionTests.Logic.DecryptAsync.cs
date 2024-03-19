// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.Suppliers;
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
            SubscriberCredential subscriberCredential = CreateRandomSubscriberCredential();
            Supplier supplier = CreateRandomSupplier(randomDateTimeOffset);
            DataSet activeDataSet = CreateRandomDataSet(supplier.Id);
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);
            string blobContainer = "emislanding";
            SubscriberAgreement subscriberAgreement = CreateRandomSubscriberAgreement(randomDateTimeOffset);
            SubscriberAgreement storageSubscriberAgreement = await this.storageBroker.InsertSubscriberAgreementAsync(subscriberAgreement);
            string fileName = CreateRandomFilePath(storageSubscriberAgreement.Id);
            string encryptedFileName = CreateRandomEncryptedFilePath(storageSubscriberAgreement.Id, storageSubscriberAgreement.SupplierSharingAgreementGuid);
            string decryptedFileName = CreateRandomDecryptedFilePath(
                activeDataSet.DataSetName,
                activeDataSetSpecification.Id,
                fileName.Split('_')[3],
                storageSubscriberAgreement.Id,
                storageSubscriberAgreement.SupplierSharingAgreementGuid);

            string randomString = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);
            byte[] encryptedData = await this.cryptographyProvider.EncryptAsync(randomBytes, subscriberCredential);

            Document document = new Document
            {
                FileName = fileName,
                DocumentData = encryptedData
            };

            string encryptedFileSha256Hash =
                this.hashBroker.GenerateSha256Hash(document.DocumentData);

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                document,
                encryptedFileName,
                decryptedFileName,
                encryptedFileSha256Hash,
                supplierId: this.landingConfiguration.LandingSupplierId);

            await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);
            await this.documentProcessingService.AddDocumentAsync(document, blobContainer);

            await this.blobStorageBroker.InsertFileAsync(
                fileName: fileName,
                stream: new MemoryStream(document.DocumentData),
                blobContainer);

            //When
            var actualString = await this.decryptionClient.DecryptAsync(ingestionTracking.EncryptedFileName);

            //Then
            actualString.Should().BeEquivalentTo(ingestionTracking.DecryptedFileName);

            //this.blobStorageBrokerMock.Verify(broker =>
            //    broker.SelectByFileNameAsync(ingestionTracking.EncryptedFileName, blobContainer),
            //        Times.Once);

            ingestionTracking.Should().NotBeNull();

            IngestionTracking decryptedIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

            //this.blobStorageBrokerMock.Verify(broker =>
            //    broker.InsertFileAsync(
            //        decryptedIngestionTracking.DecryptedFileName,
            //        It.IsAny<Stream>(),
            //        It.IsAny<string>()),
            //            Times.Once());

            //this.downloadBrokerMock.VerifyNoOtherCalls();
            //this.blobStorageBrokerMock.VerifyNoOtherCalls();

            await DeleteAudits(ingestionTracking);
            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
        }

        private async Task DeleteAudits(IngestionTracking ingestionTracking)
        {
            var auditIds = this.auditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id)
                .Select(ingestionTracking => ingestionTracking.Id)
                .ToList();

            foreach (var id in auditIds)
            {
                await this.auditService.RemoveIngestionTrackingAuditByIdAsync(id);
            }

            if (this.auditService.RetrieveAllIngestionTrackingAudits()
                .Any(audit => audit.IngestionTrackingId == ingestionTracking.Id))
            {
                await DeleteAudits(ingestionTracking);
            }
        }
    }
}
