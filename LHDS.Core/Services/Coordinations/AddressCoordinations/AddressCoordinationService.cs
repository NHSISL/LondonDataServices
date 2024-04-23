// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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

                var exceptions = new List<Exception>();
                List<ResolvedAddress> matchedAddresses = new List<ResolvedAddress>();

                foreach (var resolvedAddress in extractedResolvedAddresses)
                {
                    try
                    {
                        ResolvedAddress matchedResolvedAddress = await TryCatch(async () =>
                        {
                            ResolvedAddress matchedAddress =
                                await this.addressPersistanceOrchestrationService.
                                    MatchAndPersistResolvedAddressAsync(resolvedAddress);

                            return matchedAddress;
                        });

                        matchedAddresses.Add(matchedResolvedAddress);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to match address for {exceptions.Count} address files",
                        exceptions);



                }

                return matchedAddresses;
                //Handle errored files -> remove from in folder and add to error folder
            });

        public ValueTask<List<Address>> UploadResolvedAddresses() =>
            throw new System.NotImplementedException();
    }
}
