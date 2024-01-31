// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;

namespace LHDS.Core.Services.Orchestrations.AddressResolvings
{
    public interface IAddressResolvingOrchestrationService
    {
        ValueTask<List<Address>> ProcessAsync(List<AddressNormalisation> addressNormalisation);
    }
}
