// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Decisions;

namespace LHDS.Core.Clients
{
    public interface IIDecideClient
    {
        ValueTask<List<Decision>> GetPatientDecisions();
    }
}
