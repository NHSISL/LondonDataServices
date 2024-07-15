// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Clients
{
    public interface IAddressClient
    {
        public ValueTask LoadAddressDataAsync(Stream data, string filename);
        public ValueTask SyncAddressesWithAssign();
        public ValueTask LoadAddressesToResolveAsync(Stream data, string filename);
        public ValueTask MatchAddressDataAsync();
        public ValueTask<List<Guid>> ExportResolvedAddressesAsync();
    }
}
