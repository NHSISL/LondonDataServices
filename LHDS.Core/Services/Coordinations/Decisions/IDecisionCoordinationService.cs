// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Decisions;

namespace LHDS.Core.Services.Coordinations.Decisions
{
    public interface IDecisionCoordinationService
    {
        public ValueTask<List<Decision>> GetPatientDecisions();
    }
}
