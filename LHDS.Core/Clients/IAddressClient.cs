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
        public ValueTask<List<Address>> LoadAddressData(byte[] data, string filename);
        public ValueTask<List<ResolvedAddress>> MatchPatientAddressData(byte[] data, string filename);
        public ValueTask<List<Address>> ProcessResolvedAddressData();
    }
}
