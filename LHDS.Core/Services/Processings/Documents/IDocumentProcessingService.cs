// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Processings.Documents
{
    public interface IDocumentProcessingService
    {
        ValueTask AddDocumentAsync(Stream input, string fileName, string container);
        ValueTask RetrieveDocumentByFileNameAsync(Stream output, string fileName, string container);
        ValueTask RemoveDocumentByFileNameAsync(string fileName, string container);
        ValueTask<string> GetDownloadLinkAsync(string fileName, string container);
    }
}
