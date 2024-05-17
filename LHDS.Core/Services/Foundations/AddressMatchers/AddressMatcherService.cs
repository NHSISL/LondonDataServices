// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Force.DeepCloner;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressMatchers;

namespace LHDS.Core.Services.Foundations.AddressMatchers
{
    public partial class AddressMatcherService : IAddressMatcherService
    {
        private readonly ILoggingBroker loggingBroker;

        public AddressMatcherService(ILoggingBroker loggingBroker)
        {
            this.loggingBroker = loggingBroker;
        }

        public BestMatchEnum CheckForBestMatch(HashSet<AddressMatch> matchedAddresses) =>
            TryCatch(() =>
            {
                ValidateCheckForBestMatchArguments(matchedAddresses);

                var orderedMatches = matchedAddresses
                    .Where(x => x.MatchingCoreComponents)
                        .OrderByDescending(x => x.MatchedComponents)
                            .ToList();

                if (orderedMatches.Count() == 0)
                {
                    return BestMatchEnum.None;
                }

                bool hasSingleWinner = orderedMatches.Count() == 1
                    || orderedMatches[0].MatchedComponents
                        != orderedMatches[1].MatchedComponents;

                if (hasSingleWinner)
                {
                    return BestMatchEnum.Single;
                }

                return BestMatchEnum.Multiple;
            });

        public HashSet<AddressMatch> CalculateMatchingAddressComponents(
            IList<KeyValuePair<string, string>> addressComponents,
            HashSet<AddressMatch> possibleAddressMatches) =>
            TryCatch(() =>
            {
                ValidateCalculateArguments(addressComponents, possibleAddressMatches);
                HashSet<AddressMatch> matchedAddresses = new HashSet<AddressMatch>();

                foreach (var address in possibleAddressMatches)
                {
                    AddressMatch addressMatch = address.DeepClone();
                    var possibleAddressComponents = address.NormalisedAddressComponents;

                    addressMatch.MatchedComponents = addressComponents
                        .Intersect(possibleAddressComponents).Count();

                    addressMatch.MatchingCoreComponents =
                        CheckMatchingCorePairs(addressComponents, possibleAddressComponents);

                    matchedAddresses.Add(addressMatch);
                }

                return matchedAddresses
                        .OrderByDescending(x => x.MatchedComponents).ToHashSet();
            });

        public IList<KeyValuePair<string, string>> RemoveNonDigitCharactersFromHouseNumber(
            IList<KeyValuePair<string, string>> addressComponents) =>
            TryCatch(() =>
            {
                ValidateAddressComponents(addressComponents);

                if (!addressComponents.Any(x => x.Key == "house_number"))
                {
                    return addressComponents;
                }

                var alteredAddressComponents = addressComponents.DeepClone();
                var houseNumberKeyValuePair = alteredAddressComponents.FirstOrDefault(x => x.Key == "house_number");
                var houseNumber = houseNumberKeyValuePair.Value;
                var rx = new Regex(@"\D");
                var matches = rx.Matches(houseNumber);

                if (matches.Count > 0)
                {
                    foreach (var match in matches.ToList())
                    {
                        houseNumber = houseNumber.Replace(match.Value, "");
                    }
                };

                alteredAddressComponents.Remove(houseNumberKeyValuePair);
                alteredAddressComponents.Add(new KeyValuePair<string, string>("house_number", houseNumber));

                return alteredAddressComponents;
            });

        public IList<KeyValuePair<string, string>> TurnAddressIntoAppartment(
            IList<KeyValuePair<string, string>> addressComponents) =>
               throw new NotImplementedException();

        public IList<KeyValuePair<string, string>> TurnAddressIntoFlat(
            IList<KeyValuePair<string, string>> addressComponents) =>
               throw new NotImplementedException();


        private static bool CheckMatchingCorePairs(
            IList<KeyValuePair<string, string>> incomingAddressComponents,
            IList<KeyValuePair<string, string>> possibleAddressComponents)
        {
            bool incomingHasHouseNumber = incomingAddressComponents.Any(kv => kv.Key == "house_number");
            bool possibleAddressHasHouseNumber = possibleAddressComponents.Any(kv => kv.Key == "house_number");

            if (incomingHasHouseNumber || possibleAddressHasHouseNumber)
            {
                bool matchOnHouseNumberAndPostCode = incomingHasHouseNumber && possibleAddressHasHouseNumber &&
                    incomingAddressComponents.Any(kv => kv.Key == "house_number" &&
                        possibleAddressComponents.Any(kv2 => kv2.Key == "house_number"
                            && kv2.Value.Equals(kv.Value, StringComparison.InvariantCultureIgnoreCase))) &&
                                incomingAddressComponents.Any(kv => kv.Key == "postcode" && possibleAddressComponents
                                    .Any(kv2 => kv2.Key == "postcode" && kv2.Value
                                        .Equals(kv.Value, StringComparison.InvariantCultureIgnoreCase)));

                return matchOnHouseNumberAndPostCode;
            }
            else
            {
                bool matchOnPostcode = incomingAddressComponents.Any(kv => kv.Key == "postcode" &&
                    possibleAddressComponents.Any(kv2 => kv2.Key == "postcode"
                        && kv2.Value.Equals(kv.Value, StringComparison.InvariantCultureIgnoreCase)));

                return matchOnPostcode;
            }
        }
    }
}
