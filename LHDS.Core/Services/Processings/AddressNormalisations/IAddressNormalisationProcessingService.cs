// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressNormalisation;

namespace LHDS.Core.Services.Processings.AddressNormalisations
{
    public interface IAddressNormalisationProcessingService
    {
        ValueTask<AddressNormalisation> GetNormalisedAddress(string address);
    }
}
