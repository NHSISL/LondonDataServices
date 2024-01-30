// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Services.Foundations.AddressNormalisations;
using LHDS.Core.Services.Foundations.AddressParsers;

namespace LHDS.Core.Services.Processings.AddressParsers
{
    public partial class AddressParserProcessingService : IAddressParserProcessingService
    {
        private readonly IAddressParserService addressParserService;
        private readonly ILoggingBroker loggingBroker;

        public AddressParserProcessingService(
            IAddressParserService addressParserService,
            ILoggingBroker loggingBroker)
        {
            this.addressParserService = addressParserService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<Address>> ProcessCsvAsync(byte[] data) =>
            throw new System.NotImplementedException();

        public ValueTask<List<Address>> ProcessCsvAsync(string data) =>
            throw new System.NotImplementedException();
    }
}
