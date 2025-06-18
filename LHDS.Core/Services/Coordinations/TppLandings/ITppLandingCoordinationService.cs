// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Coordinations.TppLandings
{
    public interface ITppLandingCoordinationService
    {
        ValueTask<Guid> ProcessAsync(string fileName, Guid supplierId);
        ValueTask ReProcessAsync(Guid supplierId);
    }
}
