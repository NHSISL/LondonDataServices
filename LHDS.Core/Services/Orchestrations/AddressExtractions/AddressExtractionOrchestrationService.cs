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
using LHDS.Core.Services.Foundations.ResolvedAddressParsers;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationService : IAddressExtractionOrchestrationService
    {
        private readonly IAddressParserService addressParserService;
        private readonly IAddressNormalisationService addressNormalisationService;
        private readonly IResolvedAddressParserService resolvedAddressParserService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AddressExtractionOrchestrationService(
            IAddressParserService addressParserService,
            IAddressNormalisationService addressNormalisationService,
            IResolvedAddressParserService resolvedAddressParserService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.addressParserService = addressParserService;
            this.addressNormalisationService = addressNormalisationService;
            this.resolvedAddressParserService = resolvedAddressParserService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<List<Address>> ProcessAddressesAsync(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data, filename);

                List<Address> addresses = await this.addressParserService
                    .ProcessCsvAsync(data, filename);

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
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data);
                List<ResolvedAddress> resolvedAddresses = await this.resolvedAddressParserService.ProcessCsvAsync(data);
                List<ResolvedAddress> processedResolvedAddresses = new List<ResolvedAddress>();
                var exceptions = new List<Exception>();

                foreach (ResolvedAddress resolvedAddress in resolvedAddresses)
                {
                    try
                    {
                        ResolvedAddress processedResolvedAddress = await TryCatch(async () =>
                        {
                            ResolvedAddress inputResolvedAddress = resolvedAddress;
                            string addressString = inputResolvedAddress.UnstructuredPostalAddress;

                            AddressNormalisation addressNormalisation =
                                await this.addressNormalisationService.GetNormalisedAddress(addressString);

                            inputResolvedAddress.JsonPostalAddress = addressNormalisation.JsonPostalAddress;
                            inputResolvedAddress.PostalAddress = addressNormalisation.PostalAddress;

                            return inputResolvedAddress;
                        });

                        processedResolvedAddresses.Add(processedResolvedAddress);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to normalise address for {exceptions.Count} resolved addresses",
                        exceptions);
                }

                return processedResolvedAddresses;
            });
    }
}
