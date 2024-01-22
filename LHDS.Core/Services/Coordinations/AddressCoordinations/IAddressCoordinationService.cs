// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public interface IAddressCoordinationService
    {
        public ValueTask<List<Address>> ProcessData(byte[] data);
    }
}
