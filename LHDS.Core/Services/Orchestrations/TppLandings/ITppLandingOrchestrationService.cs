// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.TppLandings
{
    public interface ITppLandingOrchestrationService
    {
        ValueTask<Guid> ProcessAsync(string fileName, Guid supplierId);
        ValueTask<List<Guid>> ReProcessAsync(Guid supplierId);
    }
}