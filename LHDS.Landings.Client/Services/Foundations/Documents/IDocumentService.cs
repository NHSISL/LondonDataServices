// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;

namespace LHDS.Landings.Client.Services.Foundations.Documents
{
    public interface IDocumentService
    {
        ValueTask AddDocumentAsync(Document document, bool isDecrypted);
        ValueTask<Document> RetrieveDocumentByFileNameAsync(string fileName, bool isDecrypted);
        ValueTask RemoveDocumentByFileNameAsync(string filename, bool isDecrypted);
    }
}
