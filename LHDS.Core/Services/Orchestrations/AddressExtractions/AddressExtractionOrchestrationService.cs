// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Extensions.Addresses;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.AddressExtractionAudits;
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
        private readonly IAddressExtractionAuditService addressExtractionAuditService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;

        public AddressExtractionOrchestrationService(
            IAddressParserService addressParserService,
            IAddressNormalisationService addressNormalisationService,
            IResolvedAddressParserService resolvedAddressParserService,
            IAddressExtractionAuditService addressExtractionAuditService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker)
        {
            this.addressParserService = addressParserService;
            this.addressNormalisationService = addressNormalisationService;
            this.resolvedAddressParserService = resolvedAddressParserService;
            this.addressExtractionAuditService = addressExtractionAuditService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public async ValueTask<List<Address>> ProcessAddressesAsync(byte[] data) =>
           throw new NotImplementedException();

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
