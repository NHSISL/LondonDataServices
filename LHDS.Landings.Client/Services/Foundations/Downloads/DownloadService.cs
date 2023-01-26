using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Brokers.Storages;
using LHDS.Landings.Client.Models.Downloads;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DownloadService : IDownloadService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DownloadService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Download> AddDownloadAsync(Download download) =>
            TryCatch(async () =>
            {
                ValidateDownloadOnAdd(download);

                return await this.storageBroker.InsertDownloadAsync(download);
            });

        public IQueryable<Download> RetrieveAllDownloads() =>
            TryCatch(() => this.storageBroker.SelectAllDownloads());

        public ValueTask<Download> RetrieveDownloadByIdAsync(Guid downloadId) =>
            TryCatch(async () =>
            {
                ValidateDownloadId(downloadId);

                Download maybeDownload = await this.storageBroker
                    .SelectDownloadByIdAsync(downloadId);

                return maybeDownload;
            });
    }
}