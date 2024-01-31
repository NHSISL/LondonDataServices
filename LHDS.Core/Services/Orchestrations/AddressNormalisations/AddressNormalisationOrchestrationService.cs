// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Services.Processings.AddressLoadingAudits;
using LHDS.Core.Services.Processings.AddressNormalisations;
using LHDS.Core.Services.Processings.AddressParsers;

namespace LHDS.Core.Services.Orchestrations.AddressNormalisations
{
    public partial class AddressNormalisationOrchestrationService : IAddressNormalisationOrchestrationService
    {
        private readonly IAddressParserProcessingService addressParserProcessingService;
        private readonly IAddressNormalisationProcessingService addressNormalisationProcessingService;
        private readonly IAddressLoadingAuditProcessingService addressLoadingAuditProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;

        public AddressNormalisationOrchestrationService(
            IAddressParserProcessingService addressParserProcessingService,
            IAddressNormalisationProcessingService addressNormalisationProcessingService,
            IAddressLoadingAuditProcessingService addressLoadingAuditProcessingService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker)
        {
            this.addressParserProcessingService = addressParserProcessingService;
            this.addressNormalisationProcessingService = addressNormalisationProcessingService;
            this.addressLoadingAuditProcessingService = addressLoadingAuditProcessingService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask<List<AddressNormalisation>> ProcessDataAsync(string data) =>
            TryCatch(async () =>
            {
                ValidateAddressNormalisationArgs(data);

                List<Address> parsedAddress =
                    await this.addressParserProcessingService.ProcessCsvAsync(data);

                List<AddressNormalisation> processedNormalisedAddresses = new List<AddressNormalisation>();

                foreach (var address in parsedAddress)
                {
                    List<string> addressList = new List<string> {
                    address.OrganisationName,
                    address.DepartmentName,
                    address.SubBuildingName,
                    address.BuildingName,
                    address.BuildingNumber,
                    address.DependentThoroughfare,
                    address.Thoroughfare,
                    address.DoubleDependentLocality,
                    address.DependentLocality,
                    address.PostTown,
                    address.PostCode
                };

                    var stringAddress = string.Join("", addressList.Where(s => !string.IsNullOrEmpty(s)));

                    AddressNormalisation normalisedAddress =
                        await this.addressNormalisationProcessingService.GetNormalisedAddress(stringAddress);

                    processedNormalisedAddresses.Add(normalisedAddress);

                    var addressLoadingAudit = new AddressLoadingAudit
                    {
                        Id = Guid.NewGuid(),
                        CorrelationId = Guid.NewGuid(),
                        FileName = "",
                        Message = "Normalised Adderess",
                        MessageId = "",
                        CreatedBy = "System",
                        UpdatedBy = "System",
                        UpdatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset(),
                        CreatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset(),
                    };

                    await this.addressLoadingAuditProcessingService
                        .AddAddressLoadingAuditAsync(addressLoadingAudit);
                }

                return processedNormalisedAddresses;
            });
    }
}
