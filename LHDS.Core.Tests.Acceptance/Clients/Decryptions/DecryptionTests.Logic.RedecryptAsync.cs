//// ---------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------

//using System;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FluentAssertions;
//using LHDS.Core.Models.Foundations.IngestionTrackings;
//using LHDS.Core.Models.Foundations.Suppliers;
//using LHDS.Core.Models.Processings.SubscriberCredentials;
//using Xunit;

//namespace LHDS.Core.Tests.Acceptance.Clients.Decryptions
//{
//    //public partial class DecryptionTests { 
//    //{
//        //[Fact]
//        //public async Task ShouldRedecryptDocumentsAsync()
//        //{
//        //    //Given
//        //    DateTimeOffset dateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();
//        //    Guid supplierId = Guid.NewGuid();
//        //    byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());
//        //    Stream randomStream = new MemoryStream(documentData);
//        //    Stream encryptedStream = new MemoryStream();
//        //    Stream decryptedStream = new MemoryStream();
//        //    Supplier randomSupplier = CreateRandomSupplier(supplierId, dateTimeOffset);
//        //    SubscriberCredential subscriberCredential = CreateRandomSubscriberCredential();

//        //    SubscriberCredential generatedSubscriberCredential = await this.subscriberCredentialOrchestration
//        //        .ModifyOrAddSubscriberCredentialAsync(subscriberCredential, regenerateKeys: true);

//        //    string encryptedFileName = CreateRandomFileName(subscriberCredential.Id);
//        //    string decryptedFileName = CreateRandomFileName(subscriberCredential.Id);
//        //    Stream inputStream = new MemoryStream(documentData);

//        //    await this.cryptographyProvider
//        //        .EncryptAsync(input: inputStream, encryptedStream, generatedSubscriberCredential);

//        //    await this.documentService.AddDocumentAsync(
//        //        input: encryptedStream,
//        //        fileName: encryptedFileName,
//        //        container: blobContainers.EmisLanding);

//        //    await this.supplierService.AddSupplierAsync(randomSupplier);

//        //    IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
//        //        dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
//        //        encryptedFileName,
//        //        decryptedFileName,
//        //        supplierId: supplierId);

//        //    ingestionTracking.IsDownloaded = true;
//        //    ingestionTracking.Decrypted = false;
//        //    ingestionTracking.IsProcessing = false;
//        //    ingestionTracking.RetryCount = 0;
//        //    ingestionTracking.LastAttempt = dateTimeOffset.AddMinutes(-15);
//        //    await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

//        //    //When
//        //    await this.decryptionClient.RetryDecryptAsync();

//        //    //Then
//        //    await this.documentService.RetrieveDocumentByFileNameAsync(
//        //        output: decryptedStream,
//        //        ingestionTracking.DecryptedFileName,
//        //        blobContainers.Ingress);

//        //    ReadAllBytesFromStream(decryptedStream).Should().BeEquivalentTo(documentData);

//        //    IngestionTracking decryptedIngestionTracking =
//        //        await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

//        //    decryptedIngestionTracking.Decrypted.Should().BeTrue();

//        //    var audits = this.auditService.RetrieveAllIngestionTrackingAudits()
//        //        .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id).ToList();

//        //    foreach (var audit in audits)
//        //    {
//        //        await this.auditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
//        //    }

//        //    await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

//        //    await this.subscriberCredentialOrchestration
//        //        .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: subscriberCredential.Id);

//        //    await this.supplierService.RemoveSupplierByIdAsync(supplierId: supplierId);
//        //    await this.documentService.RemoveDocumentByFileNameAsync(encryptedFileName, blobContainers.EmisLanding);

//        //    await this.documentService.RemoveDocumentByFileNameAsync(
//        //        ingestionTracking.DecryptedFileName, blobContainers.Ingress);
//        //}
//    //}
////}
