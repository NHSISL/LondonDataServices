// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Coordinations.EmisLandings
{
    public interface IEmisLandingCoordinationService
    {
        ValueTask<List<string>> ProcessAsync();
        ValueTask<string> ProcessFileAsync(string fileName);
    }
}