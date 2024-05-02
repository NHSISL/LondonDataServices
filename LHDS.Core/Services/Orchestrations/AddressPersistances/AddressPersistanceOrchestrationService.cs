// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.AddressMatchers;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using Newtonsoft.Json;

namespace LHDS.Core.Services.Orchestrations.AddressPersistances
{
    internal partial class AddressPersistanceOrchestrationService : IAddressPersistanceOrchestrationService
    {
        private readonly IAddressProcessingService addressProcessingService;
        private readonly IAddressMatcherProcessingService addressMatcherProcessingService;
        private readonly IResolvedAddressProcessingService resolvedAddressProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AddressPersistanceOrchestrationService(
            IAddressProcessingService addressProcessingService,
            IAddressMatcherProcessingService addressMatcherProcessingService,
            IResolvedAddressProcessingService resolvedAddressProcessingService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.addressProcessingService = addressProcessingService;
            this.addressMatcherProcessingService = addressMatcherProcessingService;
            this.resolvedAddressProcessingService = resolvedAddressProcessingService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<List<Address>> PersistAddressAsync(List<Address> addresses, string fileName) =>
            TryCatch(async () =>
            {
                ValidateAddressPersistenceOrchestration(addresses, fileName);
                List<Address> processedAddresses = new List<Address>();
                List<Exception> exceptions = new List<Exception>();

                foreach (var address in addresses)
                {
                    try
                    {
                        var processAddress = await TryCatch(async () =>
                        {
                            Address processAddress =
                                await this.addressProcessingService.ModifyOrAddAddressAsync(address);

                            return processAddress;
                        });

                        //await this.auditBroker.LogInformation(
                        //    auditType: "Address",
                        //    title: "Successfully loaded address from Ordinance Database",
                        //    message: $"Successfully loaded address with id: {address.Id} from file: {fileName}",
                        //    fileName,
                        //    correlationId: address.Id);
                        processedAddresses.Add(processAddress);
                    }
                    catch (Exception ex)
                    {
                        this.loggingBroker.LogError(ex);
                        exceptions.Add(ex);
                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to add or modify {exceptions.Count} address(es)",
                        exceptions);
                }

                return processedAddresses;
            });

        public ValueTask<ResolvedAddress> MatchAndPersistResolvedAddressAsync(ResolvedAddress resolvedAddresses) =>
            throw new NotImplementedException();  
    }
}
