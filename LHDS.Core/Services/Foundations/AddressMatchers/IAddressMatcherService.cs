// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using LHDS.Core.Models.Foundations.AddressMatchers;

namespace LHDS.Core.Services.Foundations.AddressMatchers
{
    public interface IAddressMatcherService
    {
        BestMatchEnum CheckForBestMatch(HashSet<AddressMatch> matchedAddresses);

        HashSet<AddressMatch> CalculateMatchingAddressComponents(
            IList<KeyValuePair<string, string>> addressComponents,
            HashSet<AddressMatch> possibleAddressMatches);

        IList<KeyValuePair<string, string>> RemoveNonDigitCharactersFromHouseNumber(
            IList<KeyValuePair<string, string>> addressComponents);

        IList<KeyValuePair<string, string>> TurnAddressIntoFlat(
            IList<KeyValuePair<string, string>> addressComponents);

        IList<KeyValuePair<string, string>> TurnAddressIntoAppartment(
            IList<KeyValuePair<string, string>> addressComponents);
    }
}
