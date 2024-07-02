// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Downloads;

namespace LHDS.Core.Services.Processings.Downloads
{
    public interface IDownloadProcessingService
    {
        ValueTask<List<string>> RetrieveListOfDownloadsToProcessAsync(Download download);
        ValueTask RetrieveDownloadByFileNameAsync(Download download);
    }
}