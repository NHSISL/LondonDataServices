// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Landings.Client.Models.Audits;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessNewDocumentsAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            var isDecrypted = false;
            List<Document> randomDocuments = CreateRandomDocuments();
            List<Document> externalDocuments = randomDocuments;

            IngestionTracking externalIngestionTrackingFound = null;

            this.downloadServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync())
                   .ReturnsAsync(externalDocuments);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingServiceMock.Setup(service =>
                    service.RemoveIngestionTrackingByIdAsync(document.FileName))
                        .ReturnsAsync(externalIngestionTrackingFound);

                this.downloadServiceMock.Setup(service =>
                  service.RetrieveDownloadByFileNameAsync(document.FileName))
                      .ReturnsAsync(document);

                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Returns(randomDateTime);

                IngestionTracking newIngestionTracking =
                    new IngestionTracking
                    {
                        Id = document.FileName,
                        EncryptedFileName = $"encrypted\\{document.FileName}",
                        DecryptedFileName = $"decrypted\\{document.FileName}",
                        Decrypted = false,
                        CreatedDate = randomDateTime,
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
                    service.RemoveIngestionTrackingByIdAsync(document.FileName),
                        Times.Once);

                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Exactly(externalDocuments.Count));

                IngestionTracking newIngestionTracking =
                  new IngestionTracking
                  {
                      Id = document.FileName,
                      EncryptedFileName = $"encrypted\\{document.FileName}",
                      DecryptedFileName = $"decrypted\\{document.FileName}",
                      Decrypted = false,
                      CreatedDate = randomDateTime,
                  };

                IngestionTracking storageIngestionTracking = newIngestionTracking.DeepClone();

                this.ingestionTrackingServiceMock.Verify(service =>
                    service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                        storageIngestionTracking))),
                            Times.Once);

                this.downloadServiceMock.Verify(service =>
                    service.RetrieveDownloadByFileNameAsync(document.FileName),
                        Times.Once);

                this.documentServiceMock.Verify(service =>
                    service.AddDocumentAsync(document, isDecrypted),
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
            var isDecrypted = false;
            List<Document> randomDocuments = CreateRandomDocuments();
            List<Document> externalDocuments = randomDocuments;
            IngestionTracking externalIngestionTrackingFound = CreateRandomIngestionTracking(randomDateTime);

            this.downloadServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync())
                   .ReturnsAsync(externalDocuments);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingServiceMock.Setup(service =>
                    service.RemoveIngestionTrackingByIdAsync(document.FileName))
                        .ReturnsAsync(externalIngestionTrackingFound);

                this.downloadServiceMock.Setup(service =>
                   service.RetrieveDownloadByFileNameAsync(document.FileName))
                       .ReturnsAsync(document);

                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Returns(randomDateTime);

                IngestionTracking newIngestionTracking =
                    new IngestionTracking
                    {
                        Id = document.FileName,
                        EncryptedFileName = $"encrypted\\{document.FileName}",
                        DecryptedFileName = $"decrypted\\{document.FileName}",
                        Decrypted = false,
                        CreatedDate = randomDateTime,
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
                    service.RemoveIngestionTrackingByIdAsync(document.FileName),
                        Times.Once);

                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Never);

                IngestionTracking newIngestionTracking =
                  new IngestionTracking
                  {
                      Id = document.FileName,
                      EncryptedFileName = $"encrypted\\{document.FileName}",
                      DecryptedFileName = $"decrypted\\{document.FileName}",
                      Decrypted = false,
                      CreatedDate = randomDateTime,
                  };

                IngestionTracking storageIngestionTracking = newIngestionTracking.DeepClone();

                this.ingestionTrackingServiceMock.Verify(service =>
                    service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                        storageIngestionTracking))),
                            Times.Never);

                this.downloadServiceMock.Verify(service =>
                   service.RetrieveDownloadByFileNameAsync(document.FileName),
                       Times.Never);

                this.documentServiceMock.Verify(service =>
                    service.AddDocumentAsync(document, isDecrypted),
                        Times.Never);

                this.auditServiceMock.Verify(service =>
                    service.AddAuditAsync(It.IsAny<Audit>()),
                        Times.Never);
            }

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
