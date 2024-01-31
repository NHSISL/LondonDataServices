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

        public BestMatchEnum CheckForBestMatch(HashSet<AddressMatch> macthedAddresses) =>
            TryCatch(() =>
            {
                ValidateCheckForBestMatchArguments(macthedAddresses);

                int matchedCount = macthedAddresses.ToList()
                    .Where(x => x.MatchingCoreComponents)
                        .OrderByDescending(x => x.MatchedComponents).Count();

                BestMatchEnum result = matchedCount switch
                {
                    0 => BestMatchEnum.None,
                    1 => BestMatchEnum.Single,
                    _ => BestMatchEnum.Multiple
                };

                return result;
            });

        public IList<KeyValuePair<string, string>> RemoveNonDigitCharactersFromHouseNumber(
            IList<KeyValuePair<string, string>> addressComponents)
        {
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
        }

        public IList<KeyValuePair<string, string>> TurnAddressIntoAppartment(
            IList<KeyValuePair<string, string>> addressComponents) =>
               throw new NotImplementedException();

        public IList<KeyValuePair<string, string>> TurnAddressIntoFlat(
            IList<KeyValuePair<string, string>> addressComponents) =>
               throw new NotImplementedException();
    }
}
