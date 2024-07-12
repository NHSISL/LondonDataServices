// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.TppLandings
{
    public interface ITppLandingOrchestrationService
    {
        ValueTask<Guid> ProcessAsync(Stream input, string fileName, Guid supplierId);
    }
}