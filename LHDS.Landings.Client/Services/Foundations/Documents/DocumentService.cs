// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Brokers.Storages.Blobs;
using LHDS.Landings.Client.Models.Foundations.Documents;
using Microsoft.Extensions.Configuration;

namespace LHDS.Landings.Client.Services.Foundations.Documents
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

        public ValueTask AddDocumentAsync(Document document, bool isDecrypted) =>
            TryCatch(async () =>
            {
                ValidateDocumentOnAdd(document);

                await this.blobStorageBroker.InsertFileAsync(
                   fileName: document.FileName,
                   stream: new MemoryStream(document.DocumentData),
                   isDecrypted: isDecrypted);
            });



        public ValueTask<Document> RetrieveDocumentByFileNameAsync(string fileName, bool isDecrypted) =>
             TryCatch(async () =>
             {
                 ValidateDocumentOnRetrieve(fileName);

                 byte[] retrievedDocument = await this.blobStorageBroker
                     .SelectByFileNameAsync(
                         fileName: fileName,
                         isDecrypted: isDecrypted);

                 var document = new Document
                 {
                     FileName = fileName,
                     DocumentData = retrievedDocument
                 };

                 return document;
             });

        public ValueTask RemoveDocumentByFileNameAsync(string fileName, bool isDecrypted) =>
           TryCatch(async () =>
           {
               ValidateDeleteArguments(fileName);
               await this.blobStorageBroker.DeleteFileAsync(fileName, isDecrypted);
           });
    }
}
