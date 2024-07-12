// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Services.Foundations.Downloads;

namespace LHDS.Core.Services.Processings.Downloads
{
    public partial class DownloadProcessingService : IDownloadProcessingService
    {
        private readonly IDownloadService downloadService;
        private readonly ILoggingBroker loggingBroker;

        public DownloadProcessingService(
            IDownloadService downloadService,
            ILoggingBroker loggingBroker)
        {
            this.downloadService = downloadService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<string>> RetrieveListOfDownloadsToProcessAsync(Download download) =>
            TryCatch(async () =>
            {
                ValidateDownloadIsNotNull(download);

                return await this.downloadService.RetrieveListOfDocumentsToProcessAsync(download);
            });

        public ValueTask RetrieveDownloadByFileNameAsync(Download download) =>
            TryCatch(async () =>
            {
                ValidateDownloadIsNotNull(download);
                await this.downloadService.RetrieveDownloadByFileNameAsync(download);
            });
    }
}