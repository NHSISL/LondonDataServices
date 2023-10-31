// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public interface IAddressExtractionOrchestrationService
    {
        public List<Address> ProcessData(byte[] data);
    }
}
