// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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
            IList<KeyValuePair<string, string>> addressComponents) =>
                throw new NotImplementedException();

        public IList<KeyValuePair<string, string>> TurnAddressIntoAppartment(
            IList<KeyValuePair<string, string>> addressComponents) =>
               throw new NotImplementedException();

        public IList<KeyValuePair<string, string>> TurnAddressIntoFlat(
            IList<KeyValuePair<string, string>> addressComponents) =>
               throw new NotImplementedException();
    }
}
