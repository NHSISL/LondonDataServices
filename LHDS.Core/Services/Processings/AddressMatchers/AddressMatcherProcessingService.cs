// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Processings.AddressMatchers;

namespace LHDS.Core.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingService : IAddressMatcherProcessingService
    {
        private readonly ILoggingBroker loggingBroker;

        public AddressMatcherProcessingService(ILoggingBroker loggingBroker)
        {
            this.loggingBroker = loggingBroker;
        }

        public string CleanAddress(string address) =>
            TryCatch(() =>
            {
                ValidateAddress(address);
                var cleanAddress = address.ToLower().Trim();
                var punctuationPattern = @"\s[,.!?-]|[,.!?;:'""](?![ ])";
                var regexPunctuation = new Regex(punctuationPattern, RegexOptions.Compiled);
                var regexSpaces = new Regex(@"[ ]{2,}", RegexOptions.Compiled);
                string previousAddress;

                do
                {
                    previousAddress = cleanAddress;

                    cleanAddress = regexPunctuation.Replace(cleanAddress, match =>
                    {
                        if (match.Value.StartsWith(" ") || match.Value.EndsWith(" "))
                        {
                            return match.Value.Trim();
                        }

                        return match.Value + " ";
                    });

                    cleanAddress = regexSpaces.Replace(cleanAddress, " ");

                } while (cleanAddress != previousAddress);

                return cleanAddress;
            });

        public string ExtractPostCode(string address) =>
            TryCatch(() =>
            {
                ValidateAddress(address);
                string pattern = @"\b([A-Z]{1,2}\d{1,2}[A-Z]?\s*\d[A-Z]{2})\b";
                HashSet<string> uniqueMatches = new HashSet<string>();

                foreach (Match match in Regex.Matches(address, pattern))
                {
                    uniqueMatches.Add(match.Value);
                }

                MatchCollection matches = Regex.Matches(string.Join(" ", uniqueMatches), pattern);
                ValidateMatches(matches);
                string extractedPostCode = matches[0].Groups[1].Value;

                return extractedPostCode;
            });

        public async ValueTask<HashSet<AddressMatch>> CalculateMacthingAddressComponents(
            IList<KeyValuePair<string, string>> incomingAddressComponents,
            HashSet<AddressMatch> possibleAddresses)
        {
            HashSet<AddressMatch> matchedAddresses = new HashSet<AddressMatch>();

            foreach (var address in possibleAddresses)
            {
                AddressMatch addressMatch = address.DeepClone();
                var possibleAddressComponents = address.AddressComponents;

                addressMatch.MatchedComponents = incomingAddressComponents
                    .Intersect(possibleAddressComponents).Count();

                addressMatch.MatchingCoreComponents =
                    CheckMatchingCorePairs(incomingAddressComponents, possibleAddressComponents);

                matchedAddresses.Add(addressMatch);
            }

            return matchedAddresses;
        }

        private static bool CheckMatchingCorePairs(
            IList<KeyValuePair<string, string>> incomingAddressComponents,
            IList<KeyValuePair<string, string>> possibleAddressComponents)
        {
            bool hasHouseNumber1 = incomingAddressComponents.Any(kv => kv.Key == "house_number");
            bool hasHouseNumber2 = possibleAddressComponents.Any(kv => kv.Key == "house_number");

            if (hasHouseNumber1 || hasHouseNumber2)
            {
                bool matchOnHouseNumberAndPostCode = hasHouseNumber1 && hasHouseNumber2 &&
                    incomingAddressComponents.Any(kv => kv.Key == "house_number" &&
                        possibleAddressComponents.Any(kv2 => kv2.Key == "house_number" && kv2.Value == kv.Value)) &&
                            incomingAddressComponents.Any(kv => kv.Key == "postcode" && possibleAddressComponents
                                .Any(kv2 => kv2.Key == "postcode" && kv2.Value == kv.Value));

                return matchOnHouseNumberAndPostCode;
            }
            else
            {
                bool matchOnPostcode = incomingAddressComponents.Any(kv => kv.Key == "postcode" &&
                    possibleAddressComponents.Any(kv2 => kv2.Key == "postcode" && kv2.Value == kv.Value));

                return matchOnPostcode;
            }
        }
    }
}