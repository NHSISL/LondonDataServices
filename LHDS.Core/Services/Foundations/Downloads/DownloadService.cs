// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Downloads;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Downloads;

namespace LHDS.Core.Services.Foundations.Downloads
{
    public partial class DownloadService : IDownloadService
    {
        private readonly IDownloadBroker downloadBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DownloadService(
            IDownloadBroker downloadBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.downloadBroker = downloadBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<string>> RetrieveListOfDocumentsToProcessAsync(Download download) =>
            TryCatch(async () =>
            {
                ValidateOnRetrieveListOfDocumentsToProcessAsync(download);

                return await this.downloadBroker.GetListOfDownloadsToProcessAsync(download);
            });

        public ValueTask RetrieveDownloadByFileNameAsync(Download download) =>
            TryCatch(async () =>
            {
                ValidateOnRetrieveDownloadByFileName(download);
                await this.downloadBroker.GetDownloadByFileNameAsync(download);
            });
    }
}