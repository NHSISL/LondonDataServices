// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public interface IAddressExtractionOrchestrationService
    {
        public Task<List<Address>> ProcessDataAsync(byte[] data);
    }
}
