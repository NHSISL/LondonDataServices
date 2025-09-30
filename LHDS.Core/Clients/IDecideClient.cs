// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Decisions;

namespace LHDS.Core.Clients
{
    public class IDecideClient : IIDecideClient
    {
        public ValueTask<List<Decision>> GetPatientDecisions()
        {
            throw new System.NotImplementedException();
        }
    }
}
