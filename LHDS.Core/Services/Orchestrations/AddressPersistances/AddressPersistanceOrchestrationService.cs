// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.AddressNormalisations;

namespace LHDS.Core.Services.Orchestrations.AddressPersistances
{
    internal partial class AddressPersistanceOrchestrationService : IAddressPersistanceOrchestrationService
    {
        private readonly IAddressProcessingService addressProcessingService;
        private readonly IAddressNormalisationProcessingService addressNormalisationProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AddressPersistanceOrchestrationService(
            IAddressProcessingService addressProcessingService,
            IAddressNormalisationProcessingService addressNormalisationProcessingService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.addressProcessingService = addressProcessingService;
            this.addressNormalisationProcessingService = addressNormalisationProcessingService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<ResolvedAddress> MatchAndPersistResolvedAddressAsync(ResolvedAddress resolvedAddresses) =>
            throw new NotImplementedException();

        public ValueTask<List<Address>> PersistAddressAsync(List<Address> addresses) =>
            TryCatch(async () =>
            {
                ValidateAddressListOrchestrationOnProcess(addresses);
                List<Address> processedAddresses = new List<Address>();

                foreach (var address in addresses)
                {
                    var stringAddress = $"{address.OrganisationName},{address.DepartmentName}," +
                        $"{address.SubBuildingName},{address.BuildingName},{address.BuildingNumber}," +
                        $"{address.DependentThoroughfare},{address.Thoroughfare}," +
                        $"{address.DoubleDependentLocality}," +
                        $"{address.DependentLocality},{address.PostTown},{address.PostCode.Replace(" ", "")}";

                    AddressNormalisation normalisedAddress =
                        await this.addressNormalisationProcessingService.GetNormalisedAddress(stringAddress);

                    address.PostalAddress = normalisedAddress.PostalAddress;
                    address.JsonPostalAddress = normalisedAddress.JsonPostalAddress;
                    Address processedAddress = await this.addressProcessingService.ModifyOrAddAddressAsync(address);

                    var audit = new AddressLoadingAudit
                    {
                        Id = Guid.NewGuid(),
                        CorrelationId = Guid.NewGuid(),
                        FileName = "",
                        Message = "Success",
                        MessageId = "",
                        CreatedBy = "System",
                        UpdatedBy = "System",
                        UpdatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset(),
                        CreatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset(),
                    };

                    processedAddresses.Add(processedAddress);
                }

                return processedAddresses;
            });
    }
}
