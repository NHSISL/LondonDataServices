// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Audits;
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
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            List<Document> randomDocuments = CreateRandomDocuments();
            List<Document> externalDocuments = randomDocuments;

            IngestionTracking externalIngestionTrackingFound = null;

            this.downloadServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync())
                   .ReturnsAsync(externalDocuments);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingServiceMock.Setup(service =>
                    service.RetrieveIngestionTrackingByIdAsync(document.FileName))
                        .ReturnsAsync(externalIngestionTrackingFound);

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
                      Id = document.FileName,
                      EncryptedFileName = $"/encrypted{filename}",
                      DecryptedFileName = 
                        $"/decrypted{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",
                      Decrypted = false,
                      CreatedDate = randomDateTime,
                      LastSeen = randomDateTime,
                      FileDeleted = false,
                      FileCount = 0,
                      EncryptedFileSize = document.DocumentData.Length,
                      DecryptedFileSize = 0,
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
                    service.RetrieveIngestionTrackingByIdAsync(document.FileName),
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
                      Id = document.FileName,
                      EncryptedFileName = $"/encrypted{filename}",
                      DecryptedFileName =
                        $"/decrypted{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",
                      Decrypted = false,
                      CreatedDate = randomDateTime,
                      LastSeen = randomDateTime,
                      FileDeleted = false,
                      FileCount = 0,
                      EncryptedFileSize = document.DocumentData.Length,
                      DecryptedFileSize = 0,
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
            IngestionTracking externalIngestionTrackingFound = CreateRandomIngestionTracking(randomDateTime);

            this.downloadServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync())
                   .ReturnsAsync(externalDocuments);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingServiceMock.Setup(service =>
                    service.RetrieveIngestionTrackingByIdAsync(document.FileName))
                        .ReturnsAsync(externalIngestionTrackingFound);

                this.downloadServiceMock.Setup(service =>
                   service.RetrieveDownloadByFileNameAsync(document.FileName))
                       .ReturnsAsync(document);

                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Returns(randomDateTime);

                var filename = document.FileName.StartsWith('/')
                    ? document.FileName
                    : "/" + document.FileName;

                IngestionTracking storageIngestionTracking = externalIngestionTrackingFound.DeepClone();
                IngestionTracking modifiedIngestionTracking = storageIngestionTracking.DeepClone();
                modifiedIngestionTracking.LastSeen = randomDateTime;

                this.ingestionTrackingServiceMock.Setup(service =>
                    service.ModifyIngestionTrackingAsync(externalIngestionTrackingFound))
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
                IngestionTracking storageIngestionTracking = externalIngestionTrackingFound.DeepClone();
                IngestionTracking modifiedIngestionTracking = storageIngestionTracking.DeepClone();
                modifiedIngestionTracking.LastSeen = randomDateTime;

                this.ingestionTrackingServiceMock.Verify(service =>
                    service.RetrieveIngestionTrackingByIdAsync(document.FileName),
                        Times.Once);

                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Once);

                this.ingestionTrackingServiceMock.Verify(service =>
                    service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                        modifiedIngestionTracking))),
                            Times.Once);

                this.downloadServiceMock.Verify(service =>
                   service.RetrieveDownloadByFileNameAsync(document.FileName),
                       Times.Never);
            }

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}