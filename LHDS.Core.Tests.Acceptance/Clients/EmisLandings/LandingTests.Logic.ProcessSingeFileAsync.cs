// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.EmisLandings
{
    public partial class LandingTests
    {
        [Fact]
        public async Task ShouldProcessNewDocumentAsync()
        {
            // Given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();

            SubscriberCredential inputSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(randomSubscriberCredential);

            string randomFileName = GetRandomFileName(subscriberAgreementId: inputSubscriberCredential.Id);

            string randomFilePath = CreateRandomFilePath(
                subscriberAgreementId: inputSubscriberCredential.Id,
                fileName: randomFileName);

            string inputFileName = randomFilePath;
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());


            Document randomDocument = new Document
            {
                DocumentData = documentData,
                FileName = inputFileName
            };

            Download downloadListRequest = new Download
            {
                SubscriberCredential = inputSubscriberCredential
            };

            Download downloadFileRequest = new Download
            {
                Document = new Document { FileName = inputFileName },
                SubscriberCredential = inputSubscriberCredential
            };

            List<string> fileList = new List<string> { inputFileName };

            string encryptedFileName = $"/encrypted/{inputFileName}";
            string expectedString = $"/decrypted/{inputFileName}";

            // When
            var actualString = await this.landingClient.ProcessAsync(inputFileName);

            // Then
            actualString.Should().BeEquivalentTo(expectedString);

            IngestionTracking retrievedInestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByFileNameAsync(inputFileName);

            retrievedInestionTracking.CreatedDate.Should().Be(retrievedInestionTracking.UpdatedDate);

            var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == retrievedInestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(retrievedInestionTracking.Id);
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

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                randomDocument,
                supplierId: this.landingConfiguration.LandingSupplierId);

            await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

            // When
            var actualString = await this.landingClient.ProcessAsync(fileName);

            // Then
            actualString.Should().BeEquivalentTo(ingestionTracking.DecryptedFileName);

            var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            IngestionTracking modifiedIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByFileNameAsync(fileName);

            modifiedIngestionTracking.UpdatedDate.Should().BeAfter(ingestionTracking.UpdatedDate);
            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

            // TODO: Tear down the test data
        }
    }
}
