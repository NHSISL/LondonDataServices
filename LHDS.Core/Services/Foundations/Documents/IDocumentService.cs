// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Services.Foundations.Documents
{
    public interface IDocumentService
    {
        ValueTask AddDocumentAsync(Document document);
        ValueTask<Document> RetrieveDocumentByFileNameAsync(string fileName);
        ValueTask RemoveDocumentByFileNameAsync(string filename);
        ValueTask<string> GetDownloadLinkAsync(string fileName);
    }
}
