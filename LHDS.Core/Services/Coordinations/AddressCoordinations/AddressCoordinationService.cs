// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public class AddressCoordinationService : IAddressCoordinationService
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

        public async ValueTask<List<Address>> ProcessData(byte[] data)
        {
            List<Address> extractedAddress = this.addressExtractionOrchestrationService.ProcessData(data);

            return await this.addressPersistanceOrchestrationService.ProcessAsync(extractedAddress);
        }
    }
}
