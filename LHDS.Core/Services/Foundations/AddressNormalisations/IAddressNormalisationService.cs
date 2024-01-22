// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressNormalisations;

namespace LHDS.Core.Services.Foundations.AddressNormalisations
{
    public interface IAddressNormalisationService
    {
        ValueTask<AddressNormalisation> GetNormalisedAddress(string address);
    }
}
