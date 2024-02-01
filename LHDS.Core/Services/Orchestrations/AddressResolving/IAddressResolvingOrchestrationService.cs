// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Orchestrations.AddressResolvings
{
    public interface IAddressResolvingOrchestrationService
    {
        ValueTask<ResolvedAddress> ProcessAsync(string normalisedAddress);
    }
}
