// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.AddressNormalisations
{
    public class AddressNormalisationBroker : IAddressNormalisationBroker
    {
        public ValueTask<string[]> ExpandAddressAsync(string address) => new ValueTask<string[]>(throw new NotImplementedException());

        public ValueTask<List<KeyValuePair<string, string>>> ParseAddressAsync(string address) =>
            new ValueTask<List<KeyValuePair<string, string>>>(throw new NotImplementedException());
    }
}
