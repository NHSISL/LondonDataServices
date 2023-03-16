// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Services.Processings.Documents
{
    public interface IDocumentProcessingService
    {
        ValueTask AddDocumentAsync(Document document);
        ValueTask<Document> RetrieveDocumentByFileNameAsync(string fileName);
        ValueTask RemoveDocumentByFileNameAsync(string fileName);
    }
}
