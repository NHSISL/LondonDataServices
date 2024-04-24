// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Clients
{
    public interface IAddressClient
    {
        public ValueTask<List<Address>> LoadAddressDataAsync(byte[] data, string filename);
        public ValueTask<List<ResolvedAddress>> MatchPatientAddressDataAsync(byte[] data, string filename);
        public ValueTask<List<Address>> ProcessResolvedAddressDataAsync();
    }
}
