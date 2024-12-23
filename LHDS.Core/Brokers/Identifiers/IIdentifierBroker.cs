// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Identifiers
{
    public interface IIdentifierBroker
    {
        //Guid GetIdentifier();
        ValueTask<Guid> GetIdentifierAsync();
    }
}
