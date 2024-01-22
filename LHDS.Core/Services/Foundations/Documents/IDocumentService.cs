// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Services.Foundations.Documents
{
    public interface IDocumentService
    {
        ValueTask AddDocumentAsync(Document document, string container);
        ValueTask<Document> RetrieveDocumentByFileNameAsync(string fileName, string container);
        ValueTask RemoveDocumentByFileNameAsync(string filename, string container);
        ValueTask<string> GetDownloadLinkAsync(string fileName, string container);
    }
}
