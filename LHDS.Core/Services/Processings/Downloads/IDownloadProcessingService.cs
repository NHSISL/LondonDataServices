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
        ValueTask<List<Download>> RetrieveListOfDownloadsToProcessAsync(Download download);
        ValueTask<Download> RetrieveDownloadByFileNameAsync(Download download);
    }
}