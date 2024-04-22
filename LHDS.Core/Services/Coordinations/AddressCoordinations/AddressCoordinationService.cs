// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using LHDS.Core.Services.Orchestrations.AddressNormalisations;
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationService : IAddressCoordinationService
    {
        private readonly IAddressExtractionOrchestrationService addressExtractionOrchestrationService;
        private readonly IAddressNormalisationOrchestrationService addressNormalisationOrchestrationService;
        private readonly IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService;
        private readonly IResolvedAddressOrchestrationService resolvedAddressOrchestrationService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;


        public AddressCoordinationService(
            IAddressExtractionOrchestrationService addressExtractionOrchestrationService,
            IAddressNormalisationOrchestrationService addressNormalisationOrchestrationService,
            IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService,
            IResolvedAddressOrchestrationService resolvedAddressOrchestrationService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.addressExtractionOrchestrationService = addressExtractionOrchestrationService;
            this.addressNormalisationOrchestrationService = addressNormalisationOrchestrationService;
            this.addressPersistanceOrchestrationService = addressPersistanceOrchestrationService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<Address>> LoadAddressData(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data, filename);

                List<Address> extractedAddress =
                    await this.addressExtractionOrchestrationService.ProcessAddressesAsync(data, filename);

                ValidateAddressListIsNotNull(extractedAddress);

                return await this.addressPersistanceOrchestrationService.PersistAddressAsync(extractedAddress);
            });

        public ValueTask<List<Address>> MatchAddressData(byte[] data, string filename) =>
            throw new System.NotImplementedException();

        public ValueTask<List<Address>> UploadResolvedAddresses() =>
            throw new System.NotImplementedException();
    }
}
