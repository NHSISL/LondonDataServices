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

            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTime);
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = inputIngestionTracking.DeepClone();
            IngestionTracking externalIngestionTrackingFound = null;

            this.downloadServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync())
                   .ReturnsAsync(externalDocuments);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingServiceMock.Setup(service =>
                    service.RetrieveIngestionTrackingByFileNameAsync(document.FileName))
                        .ReturnsAsync(externalIngestionTrackingFound);

                this.ingestionTrackingServiceMock.Setup(service =>
                    service.AddIngestionTrackingAsync(inputIngestionTracking))
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

                this.ingestionTrackingServiceMock.Verify(service =>
                    service.AddIngestionTrackingAsync(inputIngestionTracking),
                        Times.Once);

                this.documentServiceMock.Verify(service =>
                    service.AddDocumentAsync(inputDocument),
                        Times.Once);
            }

            this.documentServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
