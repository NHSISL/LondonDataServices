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
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
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
            DataSet activeDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = fileName
            };

            List<Document> documents = new List<Document> { document };

            this.downloadBrokerMock.Setup(broker =>
                broker.GetListOfDocumentsToProcessAsync())
                    .ReturnsAsync(documents);

            this.downloadBrokerMock.Setup(broker =>
                broker.GetDocumentByFileNameAsync(fileName))
                    .ReturnsAsync(document);

            await this.dataSetService.AddDataSetAsync(activeDataSet);
            await this.dataSetSpecificationProcessingService.AddDataSetSpecificationAsync(activeDataSetSpecification);

            DataSetSpecification retrievedDataSetSpecification =
               await this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(supplierId);

            //When
            var actualStringList = await this.landingClient.ProcessAsync();

            //Then
            this.downloadBrokerMock.Verify(broker =>
                broker.GetListOfDocumentsToProcessAsync(),
                    Times.Once);

            foreach (var actualFile in actualStringList)
            {
                this.downloadBrokerMock.Verify(broker =>
                    broker.GetDocumentByFileNameAsync(fileName),
                        Times.Once);

                string expectedFile =
                    $"/{landingConfiguration.DecryptedFolder}"
                    + $"/{activeDataSet.DataSetName}"
                    + $"/{activeDataSetSpecification.Id}"
                    + $"/{fileName.Split('_')[3]}"
                    + $"/{fileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

                actualFile.Should().BeEquivalentTo(expectedFile);

                IngestionTracking ingestionTracking = this.ingestionTrackingService.RetrieveAllIngestionTrackings()
                        .FirstOrDefault(ingestionTracking => ingestionTracking.DecryptedFileName == actualFile);

                ingestionTracking.Should().NotBeNull();

                var audits = this.auditService.RetrieveAllIngestionTrackingAudits()
                    .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

                foreach (var audit in audits)
                {
                    await this.auditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
                }

                await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

                this.blobStorageBrokerMock.Verify(broker =>
                    broker.InsertFileAsync(
                        ingestionTracking.EncryptedFileName,
                        It.IsAny<Stream>(),
                         It.IsAny<string>()),
                            Times.Once());
            }

            await this.dataSetSpecificationProcessingService
                .RemoveDataSetSpecificationByIdAsync(activeDataSetSpecification.Id);

            await this.dataSetService.RemoveDataSetByIdAsync(activeDataSet.Id);
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

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = fileName
            };

            List<Document> documents = new List<Document> { document };

            this.downloadBrokerMock.Setup(broker =>
                broker.GetListOfDocumentsToProcessAsync())
                    .ReturnsAsync(documents);

            List<IngestionTracking> ingestionTrackings = await CreateRandomIngestionTrackings(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                documents,
                supplierId: landingConfiguration.LandingSupplierId);

            //When
            var actualStringList = await this.landingClient.ProcessAsync();

            //Then
            actualStringList.Should().HaveCount(0);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetListOfDocumentsToProcessAsync(),
                    Times.Once);

            foreach (var tracking in ingestionTrackings)
            {
                var audits = this.auditService.RetrieveAllIngestionTrackingAudits()
                    .Where(audit => audit.IngestionTrackingId == tracking.Id);

                foreach (var audit in audits)
                {
                    await this.auditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
                }

                await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(tracking.Id);
            }

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
