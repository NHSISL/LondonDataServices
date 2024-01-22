// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.EmisLandings
{
    public interface IEmisLandingOrchestrationService
    {
        ValueTask<List<string>> ProcessAsync();
        ValueTask<string> ProcessAsync(string fileName);
    }
}