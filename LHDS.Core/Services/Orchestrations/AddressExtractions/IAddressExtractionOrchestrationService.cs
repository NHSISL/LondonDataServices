// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public interface IAddressExtractionOrchestrationService
    {
        public ValueTask<List<Address>> BulkAddAddressesAsync(byte[] data, string filename);
        public ValueTask NormaliseAddressesAsync();
        public ValueTask<List<ResolvedAddress>> ProcessResolvedAddressesAsync(byte[] data, string filename);
    }
}
