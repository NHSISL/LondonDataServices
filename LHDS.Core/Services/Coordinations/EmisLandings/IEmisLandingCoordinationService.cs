// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Services.Coordinations.EmisLandings
{
    public interface IEmisLandingCoordinationService
    {
        ValueTask<List<string>> ProcessAsync(Guid supplierId);
        ValueTask<List<string>> RetrieveListOfDocumentsToProcessAsync(Guid subscriberAgreementId);
        ValueTask<Document> RetrieveDownloadByFileNameAsync(string fileName);
        ValueTask RedecryptDocumentByIngestionIdAsync(Guid ingestionTrackingId);
    }
}