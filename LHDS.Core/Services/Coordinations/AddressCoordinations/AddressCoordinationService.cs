// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationService : IAddressCoordinationService
    {
        private readonly IAddressExtractionOrchestrationService addressExtractionOrchestrationService;
        private readonly IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService;
        private readonly ILoggingBroker loggingBroker;

        public AddressCoordinationService(
            IAddressExtractionOrchestrationService addressExtractionOrchestrationService,
            IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService,
            ILoggingBroker loggingBroker)
        {
            this.addressExtractionOrchestrationService = addressExtractionOrchestrationService;
            this.addressPersistanceOrchestrationService = addressPersistanceOrchestrationService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<Address>> ProcessData(byte[] data) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data);

                List<Address> extractedAddress =
                    await this.addressExtractionOrchestrationService.ProcessDataAsync(data);

                ValidateAddressListIsNotNull(extractedAddress);

                return await this.addressPersistanceOrchestrationService.ProcessAsync(extractedAddress);
            });
    }
}
