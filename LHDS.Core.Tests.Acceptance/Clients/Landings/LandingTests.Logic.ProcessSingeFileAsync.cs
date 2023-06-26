// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;
using Xunit.Sdk;

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

            this.blobStorageBrokerMock.Verify(broker =>
                broker.DeleteFileAsync(fileName),
                    Times.Once());

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

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

            this.blobStorageBrokerMock.Verify(broker =>
                broker.DeleteFileAsync(fileName),
                    Times.Once());

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
