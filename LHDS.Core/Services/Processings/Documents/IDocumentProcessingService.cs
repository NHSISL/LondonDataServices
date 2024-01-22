// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Services.Processings.Documents
{
    public interface IDocumentProcessingService
    {
        ValueTask<string> AddDocumentAsync(Document document, string container);
        ValueTask<Document> RetrieveDocumentByFileNameAsync(string fileName, string container);
        ValueTask RemoveDocumentByFileNameAsync(string fileName, string container);
        ValueTask<string> GetDownloadLinkAsync(string fileName, string container);
    }
}
