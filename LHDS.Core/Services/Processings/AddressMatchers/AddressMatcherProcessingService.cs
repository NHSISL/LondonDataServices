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
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Services.Foundations.AddressMatchers;

namespace LHDS.Core.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingService : IAddressMatcherProcessingService
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IAddressMatcherService addressMatcherService;

        public AddressMatcherProcessingService(
            IAddressMatcherService addressMatcherService,
            ILoggingBroker loggingBroker)
        {
            this.addressMatcherService = addressMatcherService;
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

        public string ExtractPostCode(string? address) =>
            TryCatch(() =>
            {
                ValidateAddress(address);
                string pattern = @"\b([A-Z]{1,2}\d{1,2}[A-Z]?\s*\d[A-Z]{2})\b";
                HashSet<string> uniqueMatches = new HashSet<string>();

                foreach (Match match in Regex.Matches(address?.ToUpper() ?? "", pattern))
                {
                    uniqueMatches.Add(match.Value);
                }

                MatchCollection matches = Regex.Matches(string.Join(" ", uniqueMatches), pattern);
                ValidateMatches(matches);
                string extractedPostCode = matches[0].Groups[1].Value;

                return extractedPostCode.ToLower();
            });

        public ValueTask<HashSet<AddressMatch>> CalculateMatchingAddressComponents(
            IList<KeyValuePair<string, string>> addressComponents,
            HashSet<AddressMatch> possibleAddressMatches) =>
            TryCatch(async () =>
            {
                ValidateCalculateArguments(addressComponents, possibleAddressMatches);

                return await ValueTask.FromResult(this.addressMatcherService
                    .CalculateMatchingAddressComponents(addressComponents, possibleAddressMatches));
            });

        public ValueTask<AddressMatch> FindBestMatch(
            HashSet<AddressMatch> possibleAddressMatches,
            IList<KeyValuePair<string, string>> addressComponents) =>
            TryCatch(async () =>
            {
                ValidateCalculateArguments(addressComponents, possibleAddressMatches);

                HashSet<AddressMatch> matchedAddresses = this.addressMatcherService
                    .CalculateMatchingAddressComponents(addressComponents, possibleAddressMatches);

                BestMatchEnum matchType = this.addressMatcherService.CheckForBestMatch(matchedAddresses);

                var result = matchType switch
                {
                    BestMatchEnum.None => await DoNonMatchProcess(matchedAddresses, addressComponents),
                    BestMatchEnum.Single => await SingleMatchProcess(matchedAddresses, addressComponents),
                    _ => await MultipleMatchProcess(matchedAddresses, addressComponents)
                };

                return result;
            });

        private async ValueTask<AddressMatch> DoNonMatchProcess(
            HashSet<AddressMatch> matchedAddresses,
            IList<KeyValuePair<string, string>> addressComponents)
        {
            if (addressComponents.Count() == 0)
            {
                return await ValueTask.FromResult(
                    new AddressMatch
                    {
                        IsMatched = false,
                        BestMatch = BestMatchEnum.None
                    });
            }

            IList<KeyValuePair<string, string>> amendedAddressComponents =
                this.addressMatcherService.RemoveNonDigitCharactersFromHouseNumber(addressComponents);

            HashSet<AddressMatch> reCalculatedMatches = this.addressMatcherService
                .CalculateMatchingAddressComponents(amendedAddressComponents, matchedAddresses);

            BestMatchEnum matchType = this.addressMatcherService.CheckForBestMatch(reCalculatedMatches);

            if (matchType == BestMatchEnum.Single)
            {
                return await SingleMatchProcess(reCalculatedMatches, amendedAddressComponents);
            }

            return await ValueTask.FromResult(
                new AddressMatch
                {
                    IsMatched = false,
                    BestMatch = BestMatchEnum.None
                });
        }

        private async ValueTask<AddressMatch> SingleMatchProcess(
            HashSet<AddressMatch> matchedAddresses,
            IList<KeyValuePair<string, string>> addressComponents)
        {
            var bestMatch = matchedAddresses.ToList().Where(x => x.MatchingCoreComponents)
                .OrderByDescending(x => x.MatchedComponents).First();

            bestMatch.IsMatched = true;
            bestMatch.BestMatch = BestMatchEnum.Single;

            return await ValueTask.FromResult(bestMatch);
        }

        private async ValueTask<AddressMatch> MultipleMatchProcess(
            HashSet<AddressMatch> matchedAddresses,
            IList<KeyValuePair<string, string>> addressComponents)
        {
            var multipleMatch = matchedAddresses.ToList().Where(x => x.MatchingCoreComponents)
                .OrderByDescending(x => x.MatchedComponents).First().DeepClone();

            multipleMatch.IsMatched = true;
            multipleMatch.BestMatch = BestMatchEnum.Multiple;

            return await ValueTask.FromResult(multipleMatch);
        }
    }
}