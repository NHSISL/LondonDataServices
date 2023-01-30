// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Force.DeepCloner;
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

            List<Document> randomDocuments = CreateRandomDocuments();
            List<Document> externalDocuments = randomDocuments;

            Document randomDocument = CreateRandomDocument();
            Document inputDocument = randomDocument;
            IngestionTracking externalIngestionTrackingFound = null;

            this.downloadServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync())
                   .ReturnsAsync(externalDocuments);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingServiceMock.Setup(service =>
                    service.RetrieveIngestionTrackingByFileNameAsync(document.FileName))
                        .ReturnsAsync(externalIngestionTrackingFound);

                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Returns(randomDateTime);

                IngestionTracking newIngestionTracking =
                    new IngestionTracking
                    {
                        Id = document.FileName,
                        FileName = document.FileName,
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
                    service.RetrieveIngestionTrackingByFileNameAsync(document.FileName),
                        Times.Once);

                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Exactly(externalDocuments.Count));

                IngestionTracking newIngestionTracking =
                  new IngestionTracking
                  {
                      Id = document.FileName,
                      FileName = document.FileName,
                      Decrypted = false,
                      CreatedDate = randomDateTime,
                  };

                IngestionTracking storageIngestionTracking = newIngestionTracking.DeepClone();

                this.ingestionTrackingServiceMock.Verify(service =>
                    service.AddIngestionTrackingAsync(storageIngestionTracking),
                        Times.Once);

                this.documentServiceMock.Verify(service =>
                    service.AddDocumentAsync(inputDocument),
                        Times.Once);
            }

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
