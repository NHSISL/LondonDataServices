// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public interface IDownloadService
    {
        ValueTask<List<Document>> RetrieveListOfDocumentsToProcessAsync();
        ValueTask<Document> RetrieveDocumentByFileNameAsync(string fileName);
    }
}