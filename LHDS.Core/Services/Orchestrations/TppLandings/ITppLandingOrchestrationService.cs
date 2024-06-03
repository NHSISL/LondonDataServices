// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Services.Orchestrations.TppLandings
{
    public interface ITppLandingOrchestrationService
    {
        ValueTask<Guid> ProcessAsync(Document document, Guid supplierId);
    }
}