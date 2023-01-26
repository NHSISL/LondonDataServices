// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Models.Foundations.Documents;
using Microsoft.Extensions.Configuration;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DocumentService : IDocumentService
    {
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IConfiguration configuration;

        public DocumentService(IBlobStorageBroker blobStorageBroker, ILoggingBroker loggingBroker, IConfiguration configuration)
        {
            this.blobStorageBroker = blobStorageBroker;
            this.loggingBroker = loggingBroker;
            this.configuration = configuration;
        }

        public ValueTask AddDocumentAsync(Document document) =>
            TryCatch(async () =>
            {
                var blobContainerName = this.configuration.GetValue<string>("blobContainerName");
                ValidateDocumentOnAdd(document, blobContainerName);

                await this.blobStorageBroker.InsertFileAsync(
                   fileName: document.FileName,
                   stream: new MemoryStream(document.DocumentData),
                   container: blobContainerName);
            });

        public ValueTask<Document> RetrieveDocumentByFileNameAsync(string fileName) =>
             TryCatch(async () =>
             {
                 var blobContainerName = this.configuration.GetValue<string>("blobContainerName");
                 ValidateDocumentOnRetrieve(fileName, blobContainerName);

                 byte[] retrievedDocument = await this.blobStorageBroker
                     .SelectByFileNameAsync(
                         fileName: fileName,
                         container: blobContainerName);

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
               string container = this.configuration.GetValue<string>("blobContainerName");
               ValidateDeleteArguments(fileName, container);
               await this.blobStorageBroker.DeleteFileAsync(fileName, container);
           });
    }
}
