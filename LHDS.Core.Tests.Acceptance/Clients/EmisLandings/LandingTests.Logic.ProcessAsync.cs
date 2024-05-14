// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Tests.Acceptance.Clients.EmisLandings.Models;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.EmisLandings
{
    public partial class LandingTests
    {
        [Fact]
        public async Task ShouldProcessNewDocumentsAsync()
        {
            //Given
            CleanupDownloadFolder();
            Guid supplierId = landingConfiguration.LandingSupplierId;
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();

            SubscriberCredential inputSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(
                    subscriberCredential: randomSubscriberCredential,
                    regenerateKeys: true,
                    externalUse: false);

            DataSetSpecification retrievedDataSetSpecification =
                await this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(supplierId);

            List<DocumentSource> randomFiles = PrepareAndAddFile(
                subscriberAgreementId: inputSubscriberCredential.Id,
                dataSetSpecification: retrievedDataSetSpecification,
                createFiles: true,
                count: GetRandomNumber());

            List<string> expectedFiles = randomFiles.Select(file => file.DecryptedBlobPath).ToList();

            //When
            var actualStringList = await this.landingClient.ProcessAsync(supplierId);

            //Then
            expectedFiles.Should().BeEquivalentTo(actualStringList);

            foreach (var actualFile in actualStringList)
            {
                IngestionTracking ingestionTracking = this.ingestionTrackingService.RetrieveAllIngestionTrackings()
                    .FirstOrDefault(ingestionTracking => ingestionTracking.DecryptedFileName == actualFile);

                ingestionTracking.Should().NotBeNull();

                await this.documentService.RemoveDocumentByFileNameAsync(
                    filename: ingestionTracking.EncryptedFileName,
                    container: blobContainers.EmisLanding);

                var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                    .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

                foreach (var audit in audits)
                {
                    await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
                }

                await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

            }

            await this.subscriberCredentialOrchestration
                .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: inputSubscriberCredential.Id);

            CleanupDownloadFolder();
        }

        [Fact]
        public async Task ShouldNotProcessExistingDocumentsAsync()
        {
            //Given
            CleanupDownloadFolder();
            DateTimeOffset randomDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            Guid supplierId = landingConfiguration.LandingSupplierId;
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();

            SubscriberCredential inputSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(
                    subscriberCredential: randomSubscriberCredential,
                    regenerateKeys: true,
                    externalUse: false);

            DataSetSpecification retrievedDataSetSpecification =
                await this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(supplierId);

            List<DocumentSource> documentSources = PrepareAndAddFile(
                subscriberAgreementId: inputSubscriberCredential.Id,
                dataSetSpecification: retrievedDataSetSpecification,
                createFiles: false,
                count: GetRandomNumber());

            List<IngestionTracking> ingestionTrackings = await CreateRandomIngestionTrackings(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                documentSources,
                supplierId: landingConfiguration.LandingSupplierId);

            //When
            var actualStringList = await this.landingClient.ProcessAsync(supplierId);

            //Then
            actualStringList.Should().HaveCount(0);

            foreach (var tracking in ingestionTrackings)
            {
                var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                    .Where(audit => audit.IngestionTrackingId == tracking.Id);

                foreach (var audit in audits)
                {
                    await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
                }

                await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(tracking.Id);
            }

            await this.subscriberCredentialOrchestration
                .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: inputSubscriberCredential.Id);

            CleanupDownloadFolder();
        }
    }
}
