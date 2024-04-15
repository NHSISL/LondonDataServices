// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Serializations;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.AddressMatchers;
using LHDS.Core.Services.Processings.ResolvedAddresses;

namespace LHDS.Core.Services.Orchestrations.AddressResolvings
{
    internal partial class AddressResolvingOrchestrationService : IAddressResolvingOrchestrationService
    {
        private readonly IAddressProcessingService addressProcessingService;
        private readonly IAddressMatcherProcessingService addressMatcherProcessingService;
        private readonly IResolvedAddressProcessingService resolvedAddressProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISerializationBroker serializationBroker;

        public AddressResolvingOrchestrationService(
            IAddressProcessingService addressProcessingService,
            IAddressMatcherProcessingService addressMatcherProcessingService,
            IResolvedAddressProcessingService resolvedAddressProcessingService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            ISerializationBroker serializationBroker)
        {
            this.addressProcessingService = addressProcessingService;
            this.addressMatcherProcessingService = addressMatcherProcessingService;
            this.resolvedAddressProcessingService = resolvedAddressProcessingService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.serializationBroker = serializationBroker;
        }

        public ValueTask<AddressNormalisation> ResolvedAddressAsync(AddressNormalisation normalisedAddress) =>
            TryCatch(async () =>
            {

                ValidateNormalisedAddress(normalisedAddress);

                (bool IsMatched, Guid? ItemId) isMatch =
                    await resolvedAddressProcessingService.IsExactMatchForResolvedAddressAsync(
                        normalisedAddress.PostalAddress);

                if (isMatch.IsMatched)
                {
                    ResolvedAddress resolvedAddress =
                        await resolvedAddressProcessingService.RetrieveResolvedAddressByIdAsync((Guid)isMatch.ItemId);

                    resolvedAddress.IsProcessed = false;
                    resolvedAddress.UpdatedDate = dateTimeBroker.GetCurrentDateTimeOffset();
                    await resolvedAddressProcessingService.ModifyResolvedAddressAsync(resolvedAddress);

                    return normalisedAddress;
                }
                else
                {
                    string postCode = addressMatcherProcessingService.ExtractPostCode(normalisedAddress.PostalAddress);

                    List<Address> addressesByPostCode =
                        await addressProcessingService.RetrieveAddressByPostCodeAsync(postCode);

                    List<AddressMatch> addressesToMatch = addressesByPostCode.Select(address => new AddressMatch
                    {
                        PostalAddress = address.PostalAddress,
                        JsonPostalAddress = address.JsonPostalAddress,
                        AddressComponents = GetComponents(address.JsonPostalAddress)
                    }).ToList();

                    //AddressMatch matchedAddress =
                    //    await addressMatcherProcessingService.FindBestMatch(
                    //        addressesToMatch,
                    //        normalisedAddress.AddressComponents);

                    //ResolvedAddress finalResolvedAddress = new ResolvedAddress
                    //{
                    //    UPRN = matchedAddress.UPRN,
                    //    UPSN = matchedAddress.UPSN,
                    //    PostCode = postCode,
                    //    PostalAddress = normalisedAddress.PostalAddress,
                    //    JsonPostalAddress = normalisedAddress.JsonPostalAddress,

                    //    MatchAlgorithmUsed = (MatchAlgorithmEnum)Enum.Parse(typeof(MatchAlgorithmEnum),
                    //        ((int)matchedAddress.BestMatch).ToString()),

                    //    BestMatchType = matchedAddress.BestMatch,
                    //    IsMatched = matchedAddress.IsMatched,
                    //    MatchedWithPostalAddress = matchedAddress.PostalAddress,
                    //    MatchedWithJsonPostalAddress = matchedAddress.JsonPostalAddress,
                    //    IsProcessed = false
                    //};

                    //await resolvedAddressProcessingService.AddResolvedAddressAsync(finalResolvedAddress);

                    return normalisedAddress;
                }
            });

        private List<KeyValuePair<string, string>> GetComponents(string jsonAddress)
        {
            if (string.IsNullOrWhiteSpace(jsonAddress))
            {
                return new List<KeyValuePair<string, string>>();
            }

            var deserialized = this.serializationBroker
                .Deserialize<Dictionary<string, string>>(jsonAddress).ToList();

            return deserialized;
        }
    }
}
