// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Documents;
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

        public ValueTask<List<Document>> RetrieveListOfDocumentsToProcessAsync() =>
            throw new NotImplementedException();

        public ValueTask<Document> RetrieveDownloadByFileNameAsync(string fileName) =>
            TryCatch(async () =>
            {
                ValidateDownloadArgs(fileName);
                return await this.downloadService.RetrieveDownloadByFileNameAsync(fileName);
            });
    }
}