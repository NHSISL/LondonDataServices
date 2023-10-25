// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public class AddressCoordinationService : IAddressCoordinationService
    {
        public AddressCoordinationService(
            IAddressExtractionOrchestrationService addressExtractionOrchestrationService,
            IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService,
            ILoggingBroker loggingBroker)
        {
        }

        public List<Address> ProcessData(byte[] data) =>
            throw new NotImplementedException();
    }
}
