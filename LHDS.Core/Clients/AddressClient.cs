// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Coordinations.AddressCoordinations;

namespace LHDS.Core.Clients
{
    public class AddressClient : IAddressClient
    {
        private readonly IAddressCoordinationService addressCoordinationService;

        public AddressClient(IAddressCoordinationService addressCoordinationService)
        {
            this.addressCoordinationService = addressCoordinationService;
        }

        public List<Address> ProcessData(byte[] data) =>
            throw new NotImplementedException();
    }
}
