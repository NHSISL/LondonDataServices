// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using LHDS.Core.Models.Foundations.AddressMatchers;

namespace LHDS.Core.Services.Foundations.AddressMatchers
{
    public interface IAddressMatcherService
    {
        BestMatchEnum CheckForBestMatch(HashSet<AddressMatch> macthedAddresses);
    }
}
