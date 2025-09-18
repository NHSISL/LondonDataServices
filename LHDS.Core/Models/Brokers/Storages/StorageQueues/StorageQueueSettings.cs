// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Models.Brokers.Storages.StorageQueues
{
    public class StorageQueueSettings
    {
        public string StorageQueueServiceUri { get; set; }
        public string AzureTenantId { get; set; }
        public StorageQueues StorageQueues { get; set; }
    }
}
