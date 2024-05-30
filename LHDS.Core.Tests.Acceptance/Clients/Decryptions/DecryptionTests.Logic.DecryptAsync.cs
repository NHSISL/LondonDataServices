// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
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
            DateTimeOffset dateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();
            Guid supplierId = Guid.NewGuid();
            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());
            Supplier randomSupplier = CreateRandomSupplier(supplierId, dateTimeOffset);
            SubscriberCredential subscriberCredential = CreateRandomSubscriberCredential();

            SubscriberCredential generatedSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(subscriberCredential, regenerateKeys: true);

            string fileName = CreateRandomFileName(subscriberCredential.Id);


            byte[] encryptedData = 
                await this.cryptographyProvider.EncryptAsync(documentData, generatedSubscriberCredential);

            Document document = new Document
            {
                DocumentData = encryptedData,
                FileName = fileName
            };

            await this.supplierService.AddSupplierAsync(randomSupplier);

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                document,
                supplierId: supplierId);

            await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

            //When
            var actualString = await this.decryptionClient.DecryptAsync(fileName);

            //Then
            actualString.Should().BeEquivalentTo(ingestionTracking.DecryptedFileName);
            ingestionTracking.Should().NotBeNull();

            IngestionTracking decryptedIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
        }
    }
}
