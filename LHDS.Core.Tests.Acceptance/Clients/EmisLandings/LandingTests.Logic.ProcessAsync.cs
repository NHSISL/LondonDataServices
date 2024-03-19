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
        public async Task ShouldProcessNewDocumentsAsync()
        {
            //Given
            Guid supplierId = landingConfiguration.LandingSupplierId;
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();

            SubscriberCredential inputSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(
                    subscriberCredential: randomSubscriberCredential,
                    regenerateKeys: true,
                    externalUse: false);

            List<string> randomFiles = PrepareAndAddFile(inputSubscriberCredential.Id);

            //When
            var actualStringList = await this.landingClient.ProcessAsync();

            //Then
            //foreach (var actualFile in actualStringList)
            //{
            //    string expectedFile =
            //        $"/{landingConfiguration.DecryptedFolder}"
            //        + $"/{retrievedDataSetSpecification.DataSet.DataSetName}"
            //        + $"/{retrievedDataSetSpecification.Id}"
            //        + $"/{fileName.Split('_')[3]}"
            //        + $"/{fileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

            //    actualFile.Should().BeEquivalentTo(expectedFile);

            //IngestionTracking ingestionTracking = this.ingestionTrackingService.RetrieveAllIngestionTrackings()
            //    .FirstOrDefault(ingestionTracking => ingestionTracking.DecryptedFileName == actualFile);

            //ingestionTracking.Should().NotBeNull();

            //var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
            //    .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            //foreach (var audit in audits)
            //{
            //    await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            //}

            //await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

            // TODO: Tear down the test data
        }



        [Fact]
        public async Task ShouldNotProcessExistingDocumentsAsync()
        {
            //Given
            DateTimeOffset randomDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            string fileName = GetRandomString();
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;

            await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential);

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

            Download downloadFileResponse = new Download
            {
                Document = randomDocument,
                SubscriberCredential = inputSubscriberCredential
            };

            List<string> fileList = new List<string> { fileName };
            List<Document> documents = new List<Document> { randomDocument };

            List<IngestionTracking> ingestionTrackings = await CreateRandomIngestionTrackings(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                documents,
                supplierId: landingConfiguration.LandingSupplierId);

            //When
            var actualStringList = await this.landingClient.ProcessAsync();

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

            // TODO: Tear down the test data
            // await CleanupTestData(ingestionTrackings);
        }

        //private async Task CleanupTestData(
        //    DataSet activeDataSet,
        //    DataSetSpecification activeDataSetSpecification,
        //    SubscriberCredential inputSubscriberCredential,
        //    List<IngestionTracking> ingestionTrackings)
        //{

        //    foreach (var ingestionTracking in ingestionTrackings)
        //    {
        //        var items = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
        //            .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

        //        foreach (var audit in items)
        //        {
        //            await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
        //        }

        //        await this.ingestionTrackingService
        //            .RemoveIngestionTrackingByIdAsync(ingestionTrackingId: ingestionTracking.Id);
        //    }

        //    await this.subscriberCredentialOrchestration
        //        .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: inputSubscriberCredential.Id);

        //    // await CleanupTestData(ingestionTrackings);
        //}
    }
}
