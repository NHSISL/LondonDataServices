// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using Microsoft.Extensions.Configuration;

namespace LHDS.Core.Services.Foundations.Documents
{
    public partial class DocumentService : IDocumentService
    {
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IConfiguration configuration;

        public DocumentService(
            IBlobStorageBroker blobStorageBroker,
            ILoggingBroker loggingBroker,
            IConfiguration configuration)
        {
            this.blobStorageBroker = blobStorageBroker;
            this.loggingBroker = loggingBroker;
            this.configuration = configuration;
        }

        public ValueTask AddDocumentAsync(Document document) =>
            TryCatch(async () =>
            {
                ValidateDocumentOnAdd(document);

                await this.blobStorageBroker.InsertFileAsync(
                   fileName: document.FileName,
                   stream: new MemoryStream(document.DocumentData));
            });

        public ValueTask<Document> RetrieveDocumentByFileNameAsync(string fileName) =>
             TryCatch(async () =>
             {
                 ValidateDocumentOnRetrieve(fileName);

                 byte[] retrievedDocument = await this.blobStorageBroker
                     .SelectByFileNameAsync(fileName: fileName);

                 var document = new Document
                 {
                     FileName = fileName,
                     DocumentData = retrievedDocument
                 };

                 return document;
             });

        public ValueTask RemoveDocumentByFileNameAsync(string fileName) =>
           TryCatch(async () =>
           {
               ValidateDeleteArguments(fileName);
               await this.blobStorageBroker.DeleteFileAsync(fileName);
           });
    }
}
