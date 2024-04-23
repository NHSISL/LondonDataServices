// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.Core.Brokers.Identifiers
{
    public class IdentifierBroker : IIdentifierBroker
    {
        public Guid GetIdentifier() =>
            Guid.NewGuid();
    }
}
