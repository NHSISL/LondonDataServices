// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationService : IAddressCoordinationService
    {
        private readonly IAddressExtractionOrchestrationService addressExtractionOrchestrationService;
        private readonly IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService;
        private readonly IResolvedAddressOrchestrationService resolvedAddressOrchestrationService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;


        public AddressCoordinationService(
            IAddressExtractionOrchestrationService addressExtractionOrchestrationService,
            IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService,
            IResolvedAddressOrchestrationService resolvedAddressOrchestrationService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.addressExtractionOrchestrationService = addressExtractionOrchestrationService;
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

        public ValueTask<List<ResolvedAddress>> MatchAddressDataAsync(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data, filename);

                List<ResolvedAddress> extractedResolvedAddresses =
                    await this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(data, filename);

                List<ResolvedAddress> matchedAddresses = new List<ResolvedAddress>();

                foreach (var resolvedAddress in extractedResolvedAddresses)
                {
                    ResolvedAddress matchedAddress =
                        await this.addressPersistanceOrchestrationService.
                            MatchAndPersistResolvedAddressAsync(resolvedAddress);

                    matchedAddresses.Add(matchedAddress);
                }

                return matchedAddresses;
                //Handle errored files -> remove from in folder and add to error folder
            });

        public ValueTask<List<Address>> UploadResolvedAddresses() =>
            throw new System.NotImplementedException();
    }
}
