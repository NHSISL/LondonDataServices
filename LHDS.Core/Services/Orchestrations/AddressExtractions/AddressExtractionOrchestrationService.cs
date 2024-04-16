// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.AddressNormalisations;
using LHDS.Core.Services.Foundations.AddressParsers;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationService : IAddressExtractionOrchestrationService
    {
        private readonly IAddressParserService addressParserService;
        private readonly IAddressNormalisationService addressNormalisationService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AddressExtractionOrchestrationService(
            IAddressParserService addressParserService,
            IAddressNormalisationService addressNormalisationService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.addressParserService = addressParserService;
            this.addressNormalisationService = addressNormalisationService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async ValueTask<List<Address>> ProcessAddressesAsync(byte[] data)
        {
            List<Address> addresses = await this.addressParserService.ProcessCsvAsync(data);

            return addresses;
        }

        public ValueTask<List<ResolvedAddress>> ProcessResolvedAddressesAsync(byte[] data) =>
            throw new NotImplementedException(); 
    }
}
