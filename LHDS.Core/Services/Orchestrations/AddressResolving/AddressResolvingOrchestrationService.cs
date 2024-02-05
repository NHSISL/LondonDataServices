// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Serializations;
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

        public async ValueTask<AddressNormalisation> ResolvedAddressAsync(AddressNormalisation normalisedAddress) 
        {
           (bool IsMatched, Guid? ItemId) isMatch = 
                await resolvedAddressProcessingService.IsExactMatchForResolvedAddressAsync(normalisedAddress.PostalAddress);

            if (isMatch.IsMatched)
            {
                ResolvedAddress resolvedAddress =  
                    await resolvedAddressProcessingService.RetrieveResolvedAddressByIdAsync((Guid)isMatch.ItemId);

                resolvedAddress.IsProcessed = false;
                resolvedAddress.UpdatedDate = dateTimeBroker.GetCurrentDateTimeOffset();
                await resolvedAddressProcessingService.ModifyResolvedAddressAsync(resolvedAddress);

                return normalisedAddress;
            }
            else { 
                throw new NotImplementedException();
            }
        }
    }
}
