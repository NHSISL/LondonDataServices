// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Orchestrations.EmisLandings
{
    public interface IEmisLandingOrchestrationService
    {
        ValueTask<List<string>> ProcessAsync(SubscriberCredential subscriberCredential, Guid supplierId);
        ValueTask<List<string>> RetrieveListOfDocumentsToProcessAsync(SubscriberCredential subscriberCredential);

        ValueTask CheckForBatchReady(SubscriberCredential subscriberCredential, Guid supplierId);

        ValueTask RetrieveDownloadByFileNameAsync(
            Stream output,
            string fileName,
            SubscriberCredential
            subscriberCredential);

        ValueTask RedecryptDocumentByIngestionIdAsync(Guid ingestionTrackingId);
    }
}