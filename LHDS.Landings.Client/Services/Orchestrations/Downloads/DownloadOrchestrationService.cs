// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Services.Foundations.Documents;
using LHDS.Landings.Client.Services.Foundations.Downloads;
using LHDS.Landings.Client.Services.Foundations.IngestionTrackings;

namespace LHDS.Landings.Client.Services.Orchestrations.Download
{
    public partial class DownloadOrchestrationService : IDownloadOrchestrationService
    {
        private readonly IDocumentService documentService;
        private readonly IDownloadService downloadService;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public DownloadOrchestrationService(
            IDocumentService documentService,
            IDownloadService downloadService,
            IIngestionTrackingService ingestionTrackingService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.documentService = documentService;
            this.downloadService = downloadService;
            this.ingestionTrackingService = ingestionTrackingService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async ValueTask ProcessAsync()
        {
            List<Document> retrievedDocuments =
                await this.downloadService.RetrieveListOfDocumentsToProcessAsync();

            foreach (var document in retrievedDocuments)
            {
                IngestionTracking maybeIngestionTracking =
                    await this.ingestionTrackingService
                        .RetrieveIngestionTrackingByFileNameAsync(document.FileName);

                Document retrievedDocument = await this.downloadService.RetrieveDocumentByFileNameAsync(document.FileName);

                var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                IngestionTracking newIngestionTracking =
                    new IngestionTracking
                    {
                        Id = document.FileName,
                        FileName = document.FileName,
                        Decrypted = false,
                        CreatedDate = currentDateTime,
                    };

                await this.ingestionTrackingService.AddIngestionTrackingAsync(newIngestionTracking);
                await this.documentService.AddDocumentAsync(retrievedDocument);
            }
        }
    }
}
