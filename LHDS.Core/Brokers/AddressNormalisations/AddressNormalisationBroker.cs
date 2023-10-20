// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.AddressNormalisations
{
    public class AddressNormalisationBroker : IAddressNormalisationBroker
    {
        public ValueTask<(string PostalAddress, string JsonPostalAddress)> GetNormalisedAddress(string address)
        {
            throw new NotImplementedException();
        }
    }
}
