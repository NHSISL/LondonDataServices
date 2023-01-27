// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.Downloads;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Models.Foundations.Documents;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DownloadService : IDownloadService
    {
        private readonly IDownloadBroker downloadBroker;
        private readonly ILoggingBroker loggingBroker;

        public DownloadService(
            IDownloadBroker downloadBroker,
            ILoggingBroker loggingBroker)
        {
            this.downloadBroker = downloadBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Document> AddDownloadAsync(Document document) =>
            TryCatch(async () =>
            {
                ValidateDownloadOnAdd(document);

                return await this.storageBroker.InsertDownloadAsync(document);
            });

        public IQueryable<Download> RetrieveAllDownloads() =>
            TryCatch(() => this.storageBroker.SelectAllDownloads());

        public ValueTask<Download> RetrieveDownloadByIdAsync(Guid downloadId) =>
            TryCatch(async () =>
            {
                ValidateDownloadId(downloadId);

                Download maybeDownload = await this.storageBroker
                    .SelectDownloadByIdAsync(downloadId);

                ValidateStorageDownload(maybeDownload, downloadId);

                return maybeDownload;
            });
    }
}