// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Clients
{
    public interface IEmisLandingClient
    {
        ValueTask<List<string>> ProcessAsync(Guid supplierId);
        ValueTask<string> ProcessAsync(string fileName, Guid supplierId);
    }
}
