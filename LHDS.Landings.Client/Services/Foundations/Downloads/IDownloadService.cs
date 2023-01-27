// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public interface IDownloadService
    {
        ValueTask<List<Document>> RetrieveListOfDocumentsToProcessAsync();
        ValueTask<Document> RetrieveDownloadByFileNameAsync(string fileName);
    }
}