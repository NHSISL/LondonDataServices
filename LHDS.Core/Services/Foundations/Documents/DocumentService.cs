// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using Microsoft.Extensions.Configuration;

namespace LHDS.Core.Services.Foundations.Documents
{
    public partial class DocumentService : IDocumentService
    {
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IConfiguration configuration;

        public DocumentService(
            IBlobStorageBroker blobStorageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            IConfiguration configuration)
        {
            this.blobStorageBroker = blobStorageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.configuration = configuration;
        }

        public ValueTask AddDocumentAsync(Stream input, string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateDocumentOnAdd(input, fileName, container);
                await this.blobStorageBroker.InsertFileAsync(input, fileName, container);
            });

        public ValueTask RetrieveDocumentByFileNameAsync(Stream output, string fileName, string container) =>
             TryCatch(async () =>
             {
                 ValidateArgumentsOnRetrieve(output, fileName, container);
                 await this.blobStorageBroker.SelectByFileNameAsync(output, fileName, container);
                 ValidateStorageDocument(output, fileName);
             });

        public ValueTask RemoveDocumentByFileNameAsync(string fileName, string container) =>
           TryCatch(async () =>
           {
               ValidateDeleteArguments(fileName, container);
               await this.blobStorageBroker.DeleteFileAsync(fileName, container);
           });

        public ValueTask<string> GetDownloadLinkAsync(string fileName, string container) =>
           TryCatch(async () =>
           {
               ValidateGetDownloadLinkArguments(fileName, container);
               var expireOn = this.dateTimeBroker.GetCurrentDateTimeOffset().AddMinutes(5);

               return await this.blobStorageBroker.GetDownloadLinkAsync(fileName, container, expireOn);
           });
    }
}
