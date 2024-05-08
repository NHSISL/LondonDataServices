// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressMatchers;

namespace LHDS.Core.Services.Processings.AddressMatchers
{
    public interface IAddressMatcherProcessingService
    {
        string CleanAddress(string address);
        string ExtractPostCode(string? address);

        ValueTask<HashSet<AddressMatch>> CalculateMatchingAddressComponents(
            IList<KeyValuePair<string, string>> addressComponents,
            HashSet<AddressMatch> possibleAddressMatches);

        ValueTask<AddressMatch> FindBestMatch(
            HashSet<AddressMatch> matchedAddresses,
            IList<KeyValuePair<string, string>> addressComponents);
    }
}
