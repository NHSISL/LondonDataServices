// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public interface IAddressCoordinationService
    {
        public ValueTask<List<Address>> LoadAddressDataAsync(byte[] data, string filename);
        public ValueTask<List<ResolvedAddress>> MatchAddressDataAsync(byte[] data, string filename);
        public ValueTask<List<Address>> UploadResolvedAddressesAsync();
    }
}
