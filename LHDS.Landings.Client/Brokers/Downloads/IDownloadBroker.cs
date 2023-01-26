// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;

namespace LHDS.Landings.Client.Brokers.Downloads
{
    public interface IDownloadBroker
    {
        ValueTask<List<Document>> GetListOfDocumentsToProcessAsync();
        ValueTask<Document> GetDocumentAsync();
    }
}
