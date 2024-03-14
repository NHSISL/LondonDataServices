// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
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
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;

            Document randomDocument = new Document
            {
                DocumentData = documentData,
                FileName = fileName
            };

            Download downloadListRequest = new Download
            {
                SubscriberCredential = inputSubscriberCredential
            };

            Download downloadFileRequest = new Download
            {
                Document = new Document { FileName = fileName },
                SubscriberCredential = inputSubscriberCredential
            };

            List<string> fileList = new List<string> { fileName };

            this.downloadBrokerMock.Setup(broker =>
                broker.GetListOfDownloadsToProcessAsync(downloadListRequest))
                    .ReturnsAsync(fileList);

            string encryptedFileName = $"/encrypted/{fileName}";
            string expectedString = $"/decrypted/{fileName}";

            // When
            var actualString = await this.landingClient.ProcessAsync(fileName);

            // Then
            actualString.Should().BeEquivalentTo(expectedString);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDownloadByFileNameAsync(downloadFileRequest),
                    Times.Once());

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(encryptedFileName, It.IsAny<Stream>(), It.IsAny<string>()),
                    Times.Once());

            IngestionTracking retrievedInestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByFileNameAsync(fileName);

            retrievedInestionTracking.CreatedDate.Should().Be(retrievedInestionTracking.UpdatedDate);

            var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == retrievedInestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
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
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;

            Document randomDocument = new Document
            {
                DocumentData = documentData,
                FileName = fileName
            };

            Download downloadFileRequest = new Download
            {
                Document = new Document { FileName = fileName },
                SubscriberCredential = inputSubscriberCredential
            };

            Download downloadFileResponse = new Download
            {
                Document = randomDocument,
                SubscriberCredential = inputSubscriberCredential
            };

            this.downloadBrokerMock.Setup(broker =>
                broker.GetDownloadByFileNameAsync(downloadFileRequest))
                    .ReturnsAsync(downloadFileResponse);

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                randomDocument,
                supplierId: this.landingConfiguration.LandingSupplierId);

            await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

            // When
            var actualString = await this.landingClient.ProcessAsync(fileName);

            // Then
            actualString.Should().BeEquivalentTo(ingestionTracking.DecryptedFileName);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDownloadByFileNameAsync(downloadFileRequest),
                    Times.Once());

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(ingestionTracking.EncryptedFileName, It.IsAny<Stream>(), It.IsAny<string>()),
                    Times.Once());

            var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            IngestionTracking modifiedIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByFileNameAsync(fileName);

            modifiedIngestionTracking.UpdatedDate.Should().BeAfter(ingestionTracking.UpdatedDate);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.DeleteFileAsync(ingestionTracking.EncryptedFileName, It.IsAny<string>()),
                    Times.Once());

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
