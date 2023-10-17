// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Brokers.AddressNormalisations
{
    public interface IAddressNormalisationBroker
    {
        ValueTask<(string PostalAddress, string JsonPostalAddress)> GetNormalisedAddress(string address);
    }
}
