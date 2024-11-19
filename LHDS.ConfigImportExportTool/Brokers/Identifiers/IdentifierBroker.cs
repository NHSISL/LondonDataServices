// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.ConfigImportExportTool.Brokers.Identifiers
{
    public class IdentifierBroker : IIdentifierBroker
    {
        public Guid GetIdentifier() =>
            Guid.NewGuid();

        public async ValueTask<Guid> GetIdentifierAsync() =>
            Guid.NewGuid();
    }
}
