// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Orchestrations.AddressPersistances
{
    public interface IAddressPersistanceOrchestrationService
    {
        ValueTask ProcessAsync(Address address);
    }
}
