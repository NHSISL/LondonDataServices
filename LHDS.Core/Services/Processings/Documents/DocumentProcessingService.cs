// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
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

        public ValueTask AddDocumentAsync(Stream input, string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateDocumentProcessingOnAdd(input, fileName, container);
                await this.documentService.AddDocumentAsync(input, fileName, container);
            });

        public ValueTask RetrieveDocumentByFileNameAsync(Stream output, string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateDocumentProcessingOnRetrieve(fileName, container);
                await this.documentService.RetrieveDocumentByFileNameAsync(output, fileName, container);
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
