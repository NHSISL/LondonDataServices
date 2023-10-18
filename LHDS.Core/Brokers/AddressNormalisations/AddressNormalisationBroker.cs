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
        public ValueTask<string[]> ExpandAddress(string address) =>
            throw new NotImplementedException();

        public ValueTask<List<KeyValuePair<string, string>>> ParseAddress(string address) =>
            throw new NotImplementedException();
    }
}
