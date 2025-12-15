// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Decisions;

namespace LHDS.Core.Brokers.Decisions
{
    public interface IDecisionBroker
    {
        ValueTask<List<Decision>> GetPatientDecisions();
        ValueTask RecordAdoption(List<Decision> decisionsAdopted);
    }
}
