// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Orchestrations.AddressPersistances
{
    public interface IAddressPersistanceOrchestrationService
    {
        ValueTask<List<Address>> ProcessAsync(List<Address> addresses);
    }
}
