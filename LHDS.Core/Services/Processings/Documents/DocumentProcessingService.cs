// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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

        public ValueTask AddDocumentAsync(Document document) =>
        TryCatch(async () =>
        {
            ValidateDocumentProcessingOnAdd(document);

            await this.documentService.AddDocumentAsync(document);
        });

        public ValueTask<Document> RetrieveDocumentByFileNameAsync(string fileName) =>
        TryCatch(async () =>
        {
            ValidateDocumentProcessingOnRetrieve(fileName);

            return await this.documentService.RetrieveDocumentByFileNameAsync(fileName);
        });

        public ValueTask RemoveDocumentByFileNameAsync(string fileName) =>
        TryCatch(async () =>
        {
            ValidateDocumentProcessingOnRemove(fileName);

            await this.documentService.RemoveDocumentByFileNameAsync(fileName);
        });

        public ValueTask<string> GetDownloadLinkAsync(string fileName) =>
        TryCatch(async () =>
        {
            ValidateGetDownloadLinkArguments(fileName);

            return await this.GetDownloadLinkAsync(fileName);
        });
    }
}
