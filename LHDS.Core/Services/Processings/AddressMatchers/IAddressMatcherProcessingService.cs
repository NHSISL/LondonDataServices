// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Services.Foundations.AddressMatchers
{
    internal interface IAddressMatcherProcessingService
    {
        string CleanAddress(string address);
        string ExtractPostCode(string address);
    }
}
