// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Downloads;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Models.Foundations.Documents;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
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

        public ValueTask<List<Document>> RetrieveListOfDocumentsToProcessAsync() =>
            TryCatch(async () =>
            {
                return await this.downloadBroker.GetListOfDocumentsToProcessAsync();
            });

        public ValueTask<Document> RetrieveDownloadByFileNameAsync(string fileName) =>
            TryCatch(async () =>
            {
                ValidateDownloadArgs(fileName);
                Document maybeDocument = await this.downloadBroker.GetDocumentByFileNameAsync(fileName);

                return maybeDocument;
            });
    }
}