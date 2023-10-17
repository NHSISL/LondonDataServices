// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessNewDocumentsAsync()
        {
            // given
            Guid randomGuid = Guid.NewGuid();
            Guid supplierId = landingConfiguration.LandingSupplierId;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            List<Document> randomDocuments = CreateRandomDocuments();
            List<Document> externalDocuments = randomDocuments;
            List<IngestionTracking> externalIngestionTrackingsFound = new List<IngestionTracking>();
            DataSet randomDataSet = CreateRandomDataSet(supplierId);

            IQueryable<DataSetSpecification> randomDataSetSpecificationList =
                CreateRandomDataSetSpecifications(dataSet: randomDataSet);

            DataSetSpecification randomDataSetSpecification = randomDataSetSpecificationList.First();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomGuid);

            this.downloadServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync())
                   .ReturnsAsync(externalDocuments);

            this.dataSetSpecificationProcessingServiceMock.Setup(service =>
                service.GetActiveDataSetSpecification(supplierId))
                    .Returns(ValueTask.FromResult(randomDataSetSpecificationList.FirstOrDefault()));

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingServiceMock.Setup(service =>
                    service.RetrieveAllIngestionTrackings())
                        .Returns(externalIngestionTrackingsFound.AsQueryable());

                this.downloadServiceMock.Setup(service =>
                  service.RetrieveDownloadByFileNameAsync(document.FileName))
                      .ReturnsAsync(document);

                var filename = document.FileName.StartsWith('/')
                    ? document.FileName
                    : "/" + document.FileName;

                IngestionTracking newIngestionTracking =
                    new IngestionTracking
                    {
                        Id = randomGuid,
                        FileName = document.FileName,
                        SupplierId = landingConfiguration.LandingSupplierId,
                        EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}{filename}",

                        DecryptedFileName =
                        $"/{landingConfiguration.DecryptedFolder}"
                            + $"/{randomDataSet.DataSetName}"
                            + $"/{randomDataSetSpecification.Id}"
                            + $"/{filename.Split('_')[3]}"
                            + $"{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

                        Decrypted = false,
                        LastSeen = randomDateTime,
                        FileDeleted = false,
                        RecordCount = 0,
                        EncryptedFileSize = document.DocumentData.Length,
                        DecryptedFileSize = 0,
                        CreatedBy = "System",
                        CreatedDate = randomDateTime,
                        UpdatedBy = "System",
                        UpdatedDate = randomDateTime
                    };

                IngestionTracking storageIngestionTracking = newIngestionTracking.DeepClone();

                this.ingestionTrackingServiceMock.Setup(service =>
                    service.AddIngestionTrackingAsync(newIngestionTracking))
                        .ReturnsAsync(storageIngestionTracking);
            }

            // when
            await this.downloadOrchestrationService.ProcessAsync();

            // then
            this.downloadServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(),
                    Times.Once);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingServiceMock.Verify(service =>
                    service.RetrieveAllIngestionTrackings(),
                        Times.Exactly(2));

                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Exactly(externalDocuments.Count * 2));

                var filename = document.FileName.StartsWith('/')
                    ? document.FileName
                    : "/" + document.FileName;

                IngestionTracking newIngestionTracking =
                  new IngestionTracking
                  {
                      Id = randomGuid,
                      FileName = document.FileName,
                      SupplierId = landingConfiguration.LandingSupplierId,
                      EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}{filename}",

                      DecryptedFileName =
                        $"/{landingConfiguration.DecryptedFolder}"
                            + $"/{randomDataSet.DataSetName}"
                            + $"/{randomDataSetSpecification.Id}"
                            + $"/{filename.Split('_')[3]}"
                            + $"{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

                      Decrypted = false,
                      LastSeen = randomDateTime,
                      FileDeleted = false,
                      RecordCount = 0,
                      EncryptedFileSize = document.DocumentData.Length,
                      DecryptedFileSize = 0,
                      CreatedBy = "System",
                      CreatedDate = randomDateTime,
                      UpdatedBy = "System",
                      UpdatedDate = randomDateTime
                  };

                IngestionTracking storageIngestionTracking = newIngestionTracking.DeepClone();

                this.ingestionTrackingServiceMock.Verify(service =>
                    service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                        storageIngestionTracking))),
                            Times.Once);

                this.downloadServiceMock.Verify(service =>
                    service.RetrieveDownloadByFileNameAsync(document.FileName),
                        Times.Once);

                Document newBlobDocument = new Document
                {
                    DocumentData = document.DocumentData,
                    FileName = newIngestionTracking.EncryptedFileName
                };

                this.documentServiceMock.Verify(service =>
                    service.AddDocumentAsync(It.Is(SameDocumentAs(newBlobDocument)), It.IsAny<string>()),
                        Times.Once);

                this.auditServiceMock.Verify(service =>
                    service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                        Times.Once);
            }

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotProcessExistingDocumentsAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            List<Document> randomDocuments = CreateRandomDocuments();
            List<Document> externalDocuments = randomDocuments;

            List<IngestionTracking> externalIngestionTrackingsFound =
                CreateRandomIngestionTrackings(randomDateTime, randomDocuments);

            this.downloadServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync())
                   .ReturnsAsync(externalDocuments);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingServiceMock.Setup(service =>
                    service.RetrieveAllIngestionTrackings())
                        .Returns(externalIngestionTrackingsFound.AsQueryable());
            }

            // when
            await this.downloadOrchestrationService.ProcessAsync();

            // then
            this.downloadServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(),
                    Times.Once);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingServiceMock.Verify(service =>
                    service.RetrieveAllIngestionTrackings(),
                        Times.Exactly(2));

                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.AtLeastOnce);

                var maybeIngestionTracking = externalIngestionTrackingsFound
                    .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == document.FileName);

                maybeIngestionTracking.LastSeen = randomDateTime;

                this.ingestionTrackingServiceMock.Verify(broker =>
                    broker.ModifyIngestionTrackingAsync(maybeIngestionTracking),
                        Times.Once);
            }

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessExistingNamedDocumentsAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Document randomDocument = CreateRandomDocument();
            Document externalDocument = randomDocument;
            IngestionTracking externalIngestionTracking = CreateRandomIngestionTracking(randomDateTime);

            List<IngestionTracking> externalIngestionTrackingsFound =
                new List<IngestionTracking> { externalIngestionTracking };

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(externalIngestionTrackingsFound.AsQueryable());

            this.downloadServiceMock.Setup(service =>
                  service.RetrieveDownloadByFileNameAsync(externalIngestionTracking.FileName))
                      .ReturnsAsync(externalDocument);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            IngestionTracking updatedIngestionTracking = externalIngestionTracking.DeepClone();
            updatedIngestionTracking.LastSeen = randomDateTime;
            updatedIngestionTracking.EncryptedFileSize = externalDocument.DocumentData.Length;
            IngestionTracking outputIngestionTracking = updatedIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(updatedIngestionTracking))
                    .ReturnsAsync(outputIngestionTracking);

            // when
            await this.downloadOrchestrationService.ProcessAsync(externalIngestionTracking.FileName);

            // then
            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(externalIngestionTracking.FileName),
                    Times.Once);

            this.documentServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(
                    externalIngestionTracking.EncryptedFileName, It.IsAny<string>()),
                    Times.Once);

            Document newBlobDocument = new Document
            {
                DocumentData = randomDocument.DocumentData,
                FileName = externalIngestionTracking.EncryptedFileName
            };

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(It.Is(SameDocumentAs(newBlobDocument)), It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(2));

            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                    outputIngestionTracking))),
                        Times.Once);

            this.auditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessNewNamedDocumentsAsync()
        {
            // given
            Guid supplierId = landingConfiguration.LandingSupplierId;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Guid randomIdentifier = Guid.NewGuid();
            Document randomDocument = CreateRandomDocument();
            Document externalDocument = randomDocument;
            DataSet randomDataSet = CreateRandomDataSet(supplierId);

            IQueryable<DataSetSpecification> randomDataSetSpecificationList =
                CreateRandomDataSetSpecifications(dataSet: randomDataSet);

            DataSetSpecification randomDataSetSpecification = randomDataSetSpecificationList.First();

            var filename = randomDocument.FileName.StartsWith('/')
                ? randomDocument.FileName
                : "/" + randomDocument.FileName;

            this.dataSetSpecificationProcessingServiceMock.Setup(service =>
                service.GetActiveDataSetSpecification(supplierId))
                    .Returns(ValueTask.FromResult(randomDataSetSpecificationList.FirstOrDefault()));

            IngestionTracking newIngestionTracking = new IngestionTracking
            {
                Id = randomIdentifier,
                FileName = externalDocument.FileName,
                SupplierId = this.landingConfiguration.LandingSupplierId,
                EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}{filename}",

                DecryptedFileName =
                    $"/{landingConfiguration.DecryptedFolder}"
                    + $"/{randomDataSet.DataSetName}"
                    + $"/{randomDataSetSpecification.Id}"
                    + $"/{filename.Split('_')[3]}"
                    + $"{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

                Decrypted = false,
                LastSeen = randomDateTime,
                FileDeleted = false,
                RecordCount = 0,
                EncryptedFileSize = externalDocument.DocumentData.Length,
                DecryptedFileSize = 0,
                CreatedBy = "System",
                CreatedDate = randomDateTime,
                UpdatedBy = "System",
                UpdatedDate = randomDateTime
            };

            List<IngestionTracking> externalIngestionTrackingsFound = new List<IngestionTracking>();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(externalIngestionTrackingsFound.AsQueryable());

            this.downloadServiceMock.Setup(service =>
                  service.RetrieveDownloadByFileNameAsync(newIngestionTracking.FileName))
                      .ReturnsAsync(externalDocument);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomIdentifier);

            IngestionTracking outputIngestionTracking = newIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.AddIngestionTrackingAsync(newIngestionTracking))
                    .ReturnsAsync(outputIngestionTracking);

            // when
            await this.downloadOrchestrationService.ProcessAsync(newIngestionTracking.FileName);

            // then
            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(newIngestionTracking.FileName),
                    Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            Document newBlobDocument = new Document
            {
                DocumentData = randomDocument.DocumentData,
                FileName = newIngestionTracking.EncryptedFileName
            };

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.Is(SameDocumentAs(newBlobDocument)), It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(2));

            this.ingestionTrackingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                    outputIngestionTracking))),
                        Times.Once);

            this.auditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldMarkUnavailableFilesAsDeletedAsync()
        {
            // given
            Guid randomGuid = Guid.NewGuid();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            List<Document> externalDocuments = new List<Document>();

            List<IngestionTracking> externalIngestionTrackingsFound = new List<IngestionTracking>
            {
                new IngestionTracking
                {
                    Id = Guid.NewGuid(),
                    SupplierId = this.landingConfiguration.LandingSupplierId,
                    FileName = "test.txt",
                    EncryptedFileName = "/encrypted/test.txt",
                    DecryptedFileName = "/decrypted/test.txt",
                    Decrypted = true,
                    LastSeen = randomDateTime.AddMinutes(-20),
                    FileDeleted = false,
                }
            };

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomGuid);

            this.downloadServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync())
                   .ReturnsAsync(externalDocuments);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(externalIngestionTrackingsFound.AsQueryable());

            // when
            await this.downloadOrchestrationService.ProcessAsync();

            // then
            this.downloadServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(),
                    Times.Once);

            DateTimeOffset expireTime = randomDateTime.AddMinutes(-15);

            List<IngestionTracking> expiredIngestionTrackings = externalIngestionTrackingsFound
                .Where(ingestionTracking => ingestionTracking.LastSeen <= expireTime).ToList();

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            foreach (var ingestionTracking in expiredIngestionTrackings)
            {
                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Exactly(expiredIngestionTrackings.Count + 1));

                ingestionTracking.FileDeleted = true;
                ingestionTracking.UpdatedDate = randomDateTime;

                this.ingestionTrackingServiceMock.Verify(service =>
                    service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                        ingestionTracking))),
                            Times.Once);
            }

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}