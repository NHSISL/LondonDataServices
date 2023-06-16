// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Documents;
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
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            List<Document> randomDocuments = CreateRandomDocuments();
            List<Document> externalDocuments = randomDocuments;

            List<IngestionTracking> externalIngestionTrackingsFound =
                new List<IngestionTracking>();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomGuid);

            this.downloadServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync())
                   .ReturnsAsync(externalDocuments);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingServiceMock.Setup(service =>
                    service.RetrieveAllIngestionTrackings())
                        .Returns(externalIngestionTrackingsFound.AsQueryable());

                this.downloadServiceMock.Setup(service =>
                  service.RetrieveDownloadByFileNameAsync(document.FileName))
                      .ReturnsAsync(document);

                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Returns(randomDateTime);

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
                        Times.Once);

                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Exactly(externalDocuments.Count));

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
                    service.AddDocumentAsync(It.Is(SameDocumentAs(newBlobDocument))),
                        Times.Once);

                this.auditServiceMock.Verify(service =>
                    service.AddAuditAsync(It.IsAny<Audit>()),
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
                        Times.Once);
            }

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessNamedDocumentsAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Document randomDocument = CreateRandomDocument();
            Document externalDocument = randomDocument;
            IngestionTracking externalIngestionTracking = CreateRandomIngestionTracking(randomDateTime);
            List<IngestionTracking> externalIngestionTrackingsFound = new List<IngestionTracking> { externalIngestionTracking };

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
                service.RemoveDocumentByFileNameAsync(externalIngestionTracking.FileName),
                    Times.Once);

            Document newBlobDocument = new Document
            {
                DocumentData = randomDocument.DocumentData,
                FileName = externalIngestionTracking.EncryptedFileName
            };

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(It.Is(SameDocumentAs(newBlobDocument))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                    outputIngestionTracking))),
                        Times.Once);

            this.auditServiceMock.Verify(service =>
                service.AddAuditAsync(It.IsAny<Audit>()),
                    Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotProcessNamedDocumentsIfFileNameNotExistsAsync()
        {
            // given
            string nonExistentFileName = GetRandomMessage();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Document randomDocument = CreateRandomDocument();
            Document externalDocument = randomDocument;
            IngestionTracking externalIngestionTracking = CreateRandomIngestionTracking(randomDateTime);
            List<IngestionTracking> externalIngestionTrackingsFound = new List<IngestionTracking> { externalIngestionTracking };

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(externalIngestionTrackingsFound.AsQueryable());

            // when
            await this.downloadOrchestrationService.ProcessAsync(nonExistentFileName);

            // then
            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.documentServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(nonExistentFileName),
                    Times.Never);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Document>()),
                    Times.Never);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.auditServiceMock.Verify(service =>
                service.AddAuditAsync(It.IsAny<Audit>()),
                    Times.Never);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotProcessNamedDocumentsIfDownloadIsNullAsync()
        {
            // given
            string fileName = GetRandomMessage();
            List<IngestionTracking> ingestionTrackings = new List<IngestionTracking>();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(ingestionTrackings.AsQueryable());

            // when
            await this.downloadOrchestrationService.ProcessAsync(fileName);

            // then
            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}