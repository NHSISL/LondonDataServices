// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Services.Foundations.Downloads
{
    public interface IDownloadService
    {
        ValueTask<List<Document>> RetrieveListOfDocumentsToProcessAsync();
        ValueTask<Document> RetrieveDownloadByFileNameAsync(string fileName);
    }
}