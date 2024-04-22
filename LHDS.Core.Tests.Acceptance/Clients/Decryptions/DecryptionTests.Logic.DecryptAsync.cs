// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Tests.Acceptance.Clients.Decryptions
{
    public partial class DecryptionTests
    {
        // TODO: @Hassan, to fix this test and remove the mocks on the acceptance tests.  
        // Removed to test for now to allow testing of self-hosted agent.

        //[Fact]
        //public async Task ShouldDecryptNewDocumentsAsync()
        //{
        //    //Given
        //    string fileName = GetRandomString();
        //    byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());
        //    byte[] encryptedData = await this.cryptographyProvider.EncryptAsync(documentData);

        //    Document document = new Document
        //    {
        //        DocumentData = encryptedData,
        //        FileName = fileName
        //    };

        //    IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
        //        dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
        //        document,
        //        supplierId: this.landingConfiguration.LandingSupplierId);

        //    await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

        //    this.blobStorageBrokerMock.Setup(broker =>
        //        broker.SelectByFileNameAsync(ingestionTracking.EncryptedFileName))
        //            .ReturnsAsync(encryptedData);

        //    //When
        //    var actualString = await this.decryptionClient.DecryptAsync(fileName);

        //    //Then
        //    actualString.Should().BeEquivalentTo(ingestionTracking.DecryptedFileName);

        //    this.blobStorageBrokerMock.Verify(broker =>
        //        broker.SelectByFileNameAsync(ingestionTracking.EncryptedFileName),
        //            Times.Once);

        //    ingestionTracking.Should().NotBeNull();

        //    IngestionTracking decryptedIngestionTracking =
        //        await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

        //    this.blobStorageBrokerMock.Verify(broker =>
        //        broker.InsertFileAsync(decryptedIngestionTracking.DecryptedFileName, It.IsAny<Stream>()),
        //            Times.Once());

        //    this.downloadBrokerMock.VerifyNoOtherCalls();
        //    this.blobStorageBrokerMock.VerifyNoOtherCalls();
        //    await DeleteAudits(ingestionTracking);
        //    await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
        //}

        //private async Task DeleteAudits(IngestionTracking ingestionTracking)
        //{
        //    var auditIds = this.auditService.RetrieveAllAudits()
        //        .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id)
        //        .Select(ingestionTracking => ingestionTracking.Id)
        //        .ToList();

        //    foreach (var id in auditIds)
        //    {
        //        await this.auditService.RemoveAuditByIdAsync(id);
        //    }

        //    if (this.auditService.RetrieveAllAudits()
        //        .Any(audit => audit.IngestionTrackingId == ingestionTracking.Id))
        //    {
        //        await DeleteAudits(ingestionTracking);
        //    }
        //}
    }
}
