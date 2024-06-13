// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.AddressNormalisations
{
    public class AddressNormalisationBroker : IAddressNormalisationBroker
    {
        public async ValueTask<string[]> ExpandAddressAsync(string address) =>
            throw new NotImplementedException();

        public async ValueTask<List<KeyValuePair<string, string>>> ParseAddressAsync(string address) =>
            throw new NotImplementedException();
    }
}
