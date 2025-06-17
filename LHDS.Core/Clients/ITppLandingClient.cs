// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Core.Clients
{
    public interface ITppLandingClient
    {
        ValueTask<Guid> ProcessAsync(string fileName, Guid supplierId);
    }
}