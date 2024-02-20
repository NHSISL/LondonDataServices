// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Downloads;

namespace LHDS.Core.Services.Foundations.Downloads
{
    public interface IDownloadService
    {
        ValueTask<List<Download>> RetrieveListOfDocumentsToProcessAsync(Download download);
        ValueTask<Download> RetrieveDownloadByFileNameAsync(Download download);
    }
}