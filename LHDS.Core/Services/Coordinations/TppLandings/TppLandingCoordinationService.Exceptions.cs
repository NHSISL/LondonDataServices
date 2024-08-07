// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Coordinations.TppLandings
{
    public partial class TppLandingCoordinationService
    {
        private delegate ValueTask<Guid> ReturningGuidFunction();

        private async ValueTask<Guid> TryCatch(ReturningGuidFunction returningGuidFunction)
        {
            return await returningGuidFunction();
        }
    }
}
