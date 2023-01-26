// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.IngestionTracking;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;

            List<Document> randomDocuments = CreateRandomDocuments();
            List<Document> externalDocuments = randomDocuments;
            Document randomDocument = CreateRandomDocument();
            Document inputDocument = randomDocument;

            IngestionTracking randomIngestionTracking = CreateRandomDocument();
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking externalIngestionTrackingFound = null;

            this.downloadServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync())
                   .ReturnsAsync(externalDocuments);


            foreach (var document in randomDocuments)
            {
                this.ingestionTrackingServiceMock.Setup(service =>
                    service.RetrieveIngestionTrackingByIdAsync(inputId))
                        .ReturnsAsync(externalIngestionTrackingFound);

                this.ingestionTrackingServiceMock.Setup(service =>
                    service.AddIngestionTrackingAsync(inputIngestionTracking))
                        .ReturnsAsync(externalIngestionTrackingFound);

                this.documentServiceMock.Setup(service =>
                    service.AddDocumentAsync(inputDocument))
                        .ReturnsAsync(externalIngestionTrackingFound);
            }




            // when
            await this.downloadOrchestrationService.ProcessAsync();

            // then

            this.downloadServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(),
                    Times.Once);




            this.storageBrokerMock.Verify(service =>
                service.SelectAllBoroughs(),
                    Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
