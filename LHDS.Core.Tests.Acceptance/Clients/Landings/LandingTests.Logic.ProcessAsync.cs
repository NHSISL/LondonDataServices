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
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
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
        public async Task ShouldProcessNewDocumentsAsync()
        {
            //Given
            DateTimeOffset randomDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            Guid supplierId = landingConfiguration.LandingSupplierId;
            string fileName = GetRandomFileName();
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());
            //DataSet activeDataSet = CreateRandomDataSet(supplierId);
            //DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);
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

            this.downloadBrokerMock.Setup(broker =>
                broker.GetListOfDownloadsToProcessAsync(downloadListRequest))
                    .ReturnsAsync(fileList);

            this.downloadBrokerMock.Setup(broker =>
                broker.GetDownloadByFileNameAsync(downloadFileRequest))
                    .ReturnsAsync(downloadFileResponse);

            DataSetSpecification retrievedDataSetSpecification =
                await this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(supplierId);

            //When
            var actualStringList = await this.landingClient.ProcessAsync();

            //Then
            this.downloadBrokerMock.Verify(broker =>
                broker.GetListOfDownloadsToProcessAsync(downloadListRequest),
                    Times.Once);

            foreach (var actualFile in actualStringList)
            {
                this.downloadBrokerMock.Verify(broker =>
                    broker.GetDownloadByFileNameAsync(downloadFileRequest),
                        Times.Once);

                string expectedFile =
                    $"/{landingConfiguration.DecryptedFolder}"
                    + $"/{retrievedDataSetSpecification.DataSet.DataSetName}"
                    + $"/{retrievedDataSetSpecification.Id}"
                    + $"/{fileName.Split('_')[3]}"
                    + $"/{fileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

                actualFile.Should().BeEquivalentTo(expectedFile);

                IngestionTracking ingestionTracking = this.ingestionTrackingService.RetrieveAllIngestionTrackings()
                    .FirstOrDefault(ingestionTracking => ingestionTracking.DecryptedFileName == actualFile);

                ingestionTracking.Should().NotBeNull();

                var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                    .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

                foreach (var audit in audits)
                {
                    await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
                }

                await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

                this.blobStorageBrokerMock.Verify(broker =>
                    broker.InsertFileAsync(
                        ingestionTracking.EncryptedFileName,
                        It.IsAny<Stream>(),
                        It.IsAny<string>()),
                            Times.Once());
            }

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
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

            this.downloadBrokerMock.Setup(broker =>
                broker.GetListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(downloadListRequest))))
                    .ReturnsAsync(fileList);

            List<IngestionTracking> ingestionTrackings = await CreateRandomIngestionTrackings(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                documents,
                supplierId: landingConfiguration.LandingSupplierId);

            //When
            var actualStringList = await this.landingClient.ProcessAsync();

            //Then
            actualStringList.Should().HaveCount(0);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(downloadListRequest))),
                    Times.Once);

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

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();

            await this.subscriberCredentialOrchestration
                .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: inputSubscriberCredential.Id);

            // await CleanupTestData(ingestionTrackings);
        }

        private async Task CleanupTestData(
            DataSet activeDataSet,
            DataSetSpecification activeDataSetSpecification,
            SubscriberCredential inputSubscriberCredential,
            List<IngestionTracking> ingestionTrackings)
        {

            foreach (var ingestionTracking in ingestionTrackings)
            {
                var items = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                    .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

                foreach (var audit in items)
                {
                    await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
                }

                await this.ingestionTrackingService
                    .RemoveIngestionTrackingByIdAsync(ingestionTrackingId: ingestionTracking.Id);
            }

            await this.dataSetService.AddDataSetAsync(activeDataSet);

            await this.dataSetSpecificationProcessingService
                .AddDataSetSpecificationAsync(activeDataSetSpecification);

            await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential);
        }
    }
}
