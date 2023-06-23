// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Decryptions
{
    public partial class DecryptionTests
    {
        [Fact]
        public async Task ShouldProcessNewDocumentsAsync()
        {
            //Given
            string fileName = GetRandomString();
            string encryptedFileName = GetRandomString();
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = encryptedFileName
            };

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                document,
                supplierId: this.landingConfiguration.LandingSupplierId);

            await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

            this.downloadBrokerMock.Setup(broker =>
                broker.GetDocumentByFileNameAsync(fileName))
                    .ReturnsAsync(document);

            //When
            var actualString = await this.decryptionClient.DecryptAsync(fileName);

            //Then
            actualString.Should().BeEquivalentTo(encryptedFileName);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDocumentByFileNameAsync(fileName),
                    Times.Once);

            ingestionTracking.Should().NotBeNull();

            var audits = this.auditService.RetrieveAllAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.auditService.RemoveAuditByIdAsync(audit.Id);
            }

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(ingestionTracking.EncryptedFileName, It.IsAny<Stream>()),
                    Times.Once());

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
