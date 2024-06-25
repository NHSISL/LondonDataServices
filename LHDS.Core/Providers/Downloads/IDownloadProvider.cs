// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Downloads;

namespace LHDS.Core.Providers.Downloads
{
    public interface IDownloadProvider
    {
        string Name { get; }
        bool IsOfflineProvider { get; }
        ValueTask<List<string>> GetListOfDocumentsToProcessAsync(Download download);
        ValueTask GetDocumentByFileNameAsync(Download download);
    }
}
