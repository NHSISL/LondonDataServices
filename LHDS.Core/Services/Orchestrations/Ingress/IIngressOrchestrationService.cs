// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.Ingress
{
    public interface IIngressOrchestrationService
    {
        ValueTask ProcessDecryptedItemsForBatchCompleteAsync();
        ValueTask RollbackIngestionTrackingItemAsync(string encryptedFileName);
    }
}
