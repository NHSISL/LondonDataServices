// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressNormalisations;

namespace LHDS.Core.Services.Orchestrations.AddressNormalisations
{
    public interface IAddressNormalisationOrchestrationService
    {
        public ValueTask<List<AddressNormalisation>> ProcessDataAsync(string data);
    }
}
