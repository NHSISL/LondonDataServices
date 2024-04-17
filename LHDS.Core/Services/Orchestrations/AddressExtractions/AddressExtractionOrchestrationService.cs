// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Extensions.Addresses;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
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

        public ValueTask<List<Address>> ProcessAddressesAsync(byte[] data) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data);
                List<Address> addresses = await this.addressParserService.ProcessCsvAsync(data);
                List<Address> processedAddresses = new List<Address>();
                var exceptions = new List<Exception>();

                foreach (Address address in addresses)
                {
                    try
                    {
                        Address processedAddress = await TryCatch(async () =>
                        {
                            Address inputAddress = address;
                            string addressString = inputAddress.GetFormattedAddress();

                            AddressNormalisation addressNormalisation =
                                await this.addressNormalisationService.GetNormalisedAddress(addressString);

                            inputAddress.JsonPostalAddress = addressNormalisation.JsonPostalAddress;
                            inputAddress.PostalAddress = addressNormalisation.PostalAddress;

                            return inputAddress;
                        });

                        processedAddresses.Add(processedAddress);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to normalise address for {exceptions.Count} addresses",
                        exceptions);
                }

                return processedAddresses;
            });

        public ValueTask<List<ResolvedAddress>> ProcessResolvedAddressesAsync(byte[] data) =>
            throw new NotImplementedException();
    }
}
