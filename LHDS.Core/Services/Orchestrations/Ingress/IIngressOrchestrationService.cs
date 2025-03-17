// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.Ingress
{
    public interface IIngressOrchestrationService
    {
        ValueTask CheckForBatchCompleteAsync(Guid ingestionTrackingId);
        ValueTask RollbackIngestionTrackingItemAsync(string encryptedFileName);
    }
}
