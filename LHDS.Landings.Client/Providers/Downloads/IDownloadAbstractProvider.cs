// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;

namespace LHDS.Landings.Client.Providers.Downloads
{
    internal interface IDownloadAbstractProvider
    {
        ValueTask<List<Document>> GetListOfDocumentsToProcessAsync();
        ValueTask<Document> GetDocumentAsync();
    }
}
