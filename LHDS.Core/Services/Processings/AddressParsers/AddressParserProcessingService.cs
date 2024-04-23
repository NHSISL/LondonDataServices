// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
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

        public ValueTask<List<Address>> ProcessCsvAsync(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateAddressParserOnProcessCSV(data, filename);

                List<Address> parsedAddress =
                    await this.addressParserService.ProcessCsvAsync(data, filename);

                return parsedAddress;
            });

        public ValueTask<List<Address>> ProcessCsvAsync(string data, string filename) =>
            TryCatch(async () =>
            {
                ValidateAddressParserArgs(data, filename);

                List<Address> parsedAddress =
                    await this.addressParserService.ProcessCsvAsync(data, filename);

                return parsedAddress;
            });
    }
}
