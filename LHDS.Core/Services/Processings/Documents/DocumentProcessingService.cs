// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Services.Foundations.Documents;

namespace LHDS.Core.Services.Processings.Documents
{
    public class DocumentProcessingService : IDocumentProcessingService
    {
        private readonly IDocumentService documentService;

        public DocumentProcessingService(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        public async ValueTask AddDocumentAsync(Document document)
        {
            throw new System.NotImplementedException();
        }
    }
}
