// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Coordinations.EmisLandings
{
    public interface IEmisLandingCoordinationService
    {
        ValueTask<List<string>> ProcessAsync(Guid supplierId);
        ValueTask<List<string>> RetrieveListOfDocumentsToProcessAsync(Guid subscriberAgreementId);
        ValueTask RetrieveDownloadByFileNameAsync(Stream output, string fileName);
        ValueTask RedecryptDocumentByIngestionIdAsync(Guid ingestionTrackingId);
        ValueTask<string> ReLandDocumentByFileNameAsync(string fileName);
    }
}