// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.ComponentModel;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Services.Foundations.Documents;

namespace LHDS.Core.Services.Processings.Documents
{
    public partial class DocumentProcessingService : IDocumentProcessingService
    {
        private readonly IDocumentService documentService;
        private readonly ILoggingBroker loggingBroker;

        public DocumentProcessingService(
            IDocumentService documentService,
            ILoggingBroker loggingBroker)
        {
            this.documentService = documentService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<string> AddDocumentAsync(Document document, string container) =>
            TryCatch(async () =>
            {
                ValidateDocumentProcessingOnAdd(document, container);
                await this.documentService.AddDocumentAsync(document, container);

                return document.FileName;
            });

        public ValueTask<Document> RetrieveDocumentByFileNameAsync(string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateDocumentProcessingOnRetrieve(fileName, container);

                return await this.documentService.RetrieveDocumentByFileNameAsync(fileName, container);
            });

        public ValueTask RemoveDocumentByFileNameAsync(string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateDocumentProcessingOnRemove(fileName, container);

                await this.documentService.RemoveDocumentByFileNameAsync(fileName, container);
            });

        public ValueTask<string> GetDownloadLinkAsync(string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateGetDownloadLinkArguments(fileName, container);

                return await this.documentService.GetDownloadLinkAsync(fileName, container);
            });
    }
}
