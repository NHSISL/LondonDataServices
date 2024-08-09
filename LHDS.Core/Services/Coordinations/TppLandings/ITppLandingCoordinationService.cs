// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Coordinations.TppLandings
{
    public interface ITppLandingCoordinationService
    {
        ValueTask<Guid> ProcessAsync(Stream input, string fileName, Guid supplierId);
    }
}
