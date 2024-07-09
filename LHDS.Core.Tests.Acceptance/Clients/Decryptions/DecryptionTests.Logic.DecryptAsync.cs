// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Org.BouncyCastle.Crypto;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Decryptions
{
    public partial class DecryptionTests
    {
        [Fact]
        public async Task ShouldDecryptNewDocumentsAsync()
        {
            //Given
            DateTimeOffset dateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();
            Guid supplierId = Guid.NewGuid();
            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());
            Stream inputStream = new MemoryStream(documentData);
            Stream encryptedStream = new MemoryStream();
            Stream decryptedStream = new MemoryStream();
            Supplier randomSupplier = CreateRandomSupplier(supplierId, dateTimeOffset);
            SubscriberCredential subscriberCredential = CreateRandomSubscriberCredential();

            SubscriberCredential generatedSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(subscriberCredential, regenerateKeys: true);

            await this.cryptographyProvider.EncryptAsync(inputStream, encryptedStream, generatedSubscriberCredential);
            string fileName = CreateRandomFileName(subscriberCredential.Id);
            await this.documentService.AddDocumentAsync(encryptedStream, fileName, blobContainers.EmisLanding);
            await this.supplierService.AddSupplierAsync(randomSupplier);

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                fileName,
                supplierId: supplierId);

            await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

            //When
            var actualString = await this.decryptionClient.DecryptAsync(fileName);

            //Then
            actualString.Should().BeEquivalentTo(ingestionTracking.DecryptedFileName);

            await this.documentService.RetrieveDocumentByFileNameAsync(
                output: decryptedStream,
                fileName: ingestionTracking.DecryptedFileName,
                container: blobContainers.Ingress);

            byte[] decryptedData = ReadAllBytesFromStream(decryptedStream);
            decryptedData.Should().BeEquivalentTo(documentData);

            IngestionTracking decryptedIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

            var audits = this.auditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.auditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

            await this.subscriberCredentialOrchestration
                .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: subscriberCredential.Id);

            await this.supplierService.RemoveSupplierByIdAsync(supplierId: supplierId);
            await this.documentService.RemoveDocumentByFileNameAsync(fileName, blobContainers.EmisLanding);

            await this.documentService.RemoveDocumentByFileNameAsync(
                ingestionTracking.DecryptedFileName, blobContainers.Ingress);
        }
    }
}
