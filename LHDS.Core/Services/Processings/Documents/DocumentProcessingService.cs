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
    }
}
