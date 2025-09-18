// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Brokers.Securities;

namespace LHDS.Core.Models.Brokers.Storages.StorageQueues
{
    public class Payload<T>
    {
        public T Message { get; set; }
        public DateTimeOffset EnqueuedAtUtc { get; set; } = DateTimeOffset.UtcNow;
        public EntraUser User { get; set; }
    }
}
