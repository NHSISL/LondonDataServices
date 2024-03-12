// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
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
            SubscriberAgreement subscriberAgreement = CreateRandomSubscriberAgreement(randomDateTimeOffset);
            SubscriberAgreement storageSubscriberAgreement = subscriberAgreement;

            string fileName = CreateRandomFilePath(subscriberAgreement.SupplierSharingAgreementGuid);
            string randomContainer = GetRandomString();
            SubscriberCredential subscriberCredential = CreateRandomSubscriberCredential();
            string randomString = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);
            byte[] encryptedData = await this.cryptographyProvider.EncryptAsync(randomBytes, subscriberCredential);

            Document document = new Document
            {
                FileName = fileName,
                DocumentData = encryptedData
            };

            await this.storageBroker.InsertSubscriberAgreementAsync(subscriberAgreement);

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                document,
                supplierId: this.landingConfiguration.LandingSupplierId);

            await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.SelectByFileNameAsync(ingestionTracking.EncryptedFileName, randomContainer))
                    .ReturnsAsync(encryptedData);

            //When
            var actualString = await this.decryptionClient.DecryptAsync(fileName);

            //Then
            actualString.Should().BeEquivalentTo(ingestionTracking.DecryptedFileName);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.SelectByFileNameAsync(ingestionTracking.EncryptedFileName, randomContainer),
                    Times.Once);

            ingestionTracking.Should().NotBeNull();

            IngestionTracking decryptedIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(
                    decryptedIngestionTracking.DecryptedFileName,
                    It.IsAny<Stream>(),
                    It.IsAny<string>()),
                        Times.Once());

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();

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
