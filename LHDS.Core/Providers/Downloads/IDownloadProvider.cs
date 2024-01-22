// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Providers.Downloads
{
    public interface IDownloadProvider
    {
        string Name { get; }
        bool IsMock { get; }
        ValueTask<List<Document>> GetListOfDocumentsToProcessAsync();
        ValueTask<Document> GetDocumentByFileNameAsync(string fileName);
    }
}
