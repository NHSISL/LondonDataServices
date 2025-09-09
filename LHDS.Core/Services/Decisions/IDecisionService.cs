// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Decisions;

namespace LHDS.Core.Services.Decisions
{
    public interface IDecisionService
    {
        ValueTask<List<Decision>> GetPatientDecisions(DateTimeOffset? lastPollDate);
    }
}
