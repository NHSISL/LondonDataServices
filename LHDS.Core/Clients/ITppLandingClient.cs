// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Clients
{
    public interface ITppLandingClient
    {
        ValueTask<Guid> ProcessAsync(Stream input, string fileName, Guid supplierId);
    }
}