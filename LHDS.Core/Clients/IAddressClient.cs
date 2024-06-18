// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Clients
{
    public interface IAddressClient
    {
        public ValueTask<List<Address>> LoadAddressDataAsync(byte[] data, string filename);
        public ValueTask NormaliseAddressesAsync();
        public ValueTask MatchPatientAddressDataAsync(byte[] data, string filename);
        public ValueTask<Guid?> ProcessResolvedAddressDataAsync();
    }
}
