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

namespace LHDS.Core.Tests.Acceptance.Clients.Landings
{
    public partial class LandingTests
    {
        [Fact]
        public async Task ShouldProcessNewDocumentAsync()
        {
            // Given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            string fileName = GetRandomString();
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = fileName
            };

            this.downloadBrokerMock.Setup(broker =>
                broker.GetDocumentByFileNameAsync(fileName))
                    .ReturnsAsync(document);

            string encryptedFileName = $"/encrypted/{fileName}";
            string expectedString = $"/decrypted/{fileName}";

            // When
            var actualString = await this.landingClient.ProcessAsync(fileName);

            // Then
            actualString.Should().BeEquivalentTo(expectedString);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDocumentByFileNameAsync(fileName),
                    Times.Once());

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(encryptedFileName, It.IsAny<Stream>()),
                    Times.Once());

            IngestionTracking retrievedInestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByFileNameAsync(fileName);

            retrievedInestionTracking.CreatedDate.Should().Be(retrievedInestionTracking.UpdatedDate);

            var audits = this.auditService.RetrieveAllAudits()
                            .Where(audit => audit.IngestionTrackingId == retrievedInestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.auditService.RemoveAuditByIdAsync(audit.Id);
            }

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(retrievedInestionTracking.Id);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessExistingDocumentAsync()
        {
            // Given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            string fileName = GetRandomString();
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = fileName
            };

            this.downloadBrokerMock.Setup(broker =>
                broker.GetDocumentByFileNameAsync(fileName))
                    .ReturnsAsync(document);

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                document,
                supplierId: this.landingConfiguration.LandingSupplierId);

            await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

            // When
            var actualString = await this.landingClient.ProcessAsync(fileName);

            // Then
            actualString.Should().BeEquivalentTo(ingestionTracking.DecryptedFileName);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDocumentByFileNameAsync(fileName),
                    Times.Once());

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(ingestionTracking.EncryptedFileName, It.IsAny<Stream>()),
                    Times.Once());

            var audits = this.auditService.RetrieveAllAudits()
                            .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.auditService.RemoveAuditByIdAsync(audit.Id);
            }

            IngestionTracking modifiedIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByFileNameAsync(fileName);

            modifiedIngestionTracking.UpdatedDate.Should().BeAfter(ingestionTracking.UpdatedDate);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.DeleteFileAsync(ingestionTracking.EncryptedFileName),
                    Times.Once());

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
