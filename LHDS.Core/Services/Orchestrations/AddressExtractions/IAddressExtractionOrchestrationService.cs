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
        public ValueTask<List<Address>> ProcessAddressesAsync(byte[] data, string filename);
        public ValueTask NormaliseAddresses();
        public ValueTask<List<ResolvedAddress>> ProcessResolvedAddressesAsync(byte[] data, string filename);
    }
}
