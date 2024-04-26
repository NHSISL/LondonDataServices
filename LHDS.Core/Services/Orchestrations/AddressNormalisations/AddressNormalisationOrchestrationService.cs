// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Services.Processings.AddressNormalisations;
using LHDS.Core.Services.Processings.AddressParsers;

namespace LHDS.Core.Services.Orchestrations.AddressNormalisations
{
    public partial class AddressNormalisationOrchestrationService : IAddressNormalisationOrchestrationService
    {
        private readonly IAddressParserProcessingService addressParserProcessingService;
        private readonly IAddressNormalisationProcessingService addressNormalisationProcessingService;
        private readonly IAuditBroker auditBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;

        public AddressNormalisationOrchestrationService(
            IAddressParserProcessingService addressParserProcessingService,
            IAddressNormalisationProcessingService addressNormalisationProcessingService,
            IAuditBroker auditBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker)
        {
            this.addressParserProcessingService = addressParserProcessingService;
            this.addressNormalisationProcessingService = addressNormalisationProcessingService;
            this.auditBroker = auditBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask<List<AddressNormalisation>> ProcessDataAsync(string data, string fileName) =>
            TryCatch(async () =>
            {
                ValidateAddressNormalisationArgs(data, fileName);

                List<Address> parsedAddress =
                    await this.addressParserProcessingService.ProcessCsvAsync(data, string.Empty);

                List<AddressNormalisation> processedNormalisedAddresses = new List<AddressNormalisation>();
                List<Exception> exceptions = new List<Exception>();

                foreach (var address in parsedAddress)
                {
                    try
                    {
                        var normalisedAddress = await TryCatch(async () =>
                        {
                            List<string> addressList = new List<string>
                            {
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

                            await this.auditBroker.LogInformation(
                                auditType: "Address",
                                title: "Successfully normalised address from Ordinance Database",
                                message: $"Successfully normalised address with id: {address.Id} from file: {fileName}",
                                fileName,
                                correlationId: address.Id);

                            return normalisedAddress;
                        });

                        processedNormalisedAddresses.Add(normalisedAddress);
                    }
                    catch (Exception ex)
                    {
                        this.loggingBroker.LogError(ex);
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more records failed processing.", exceptions);
                }

                return processedNormalisedAddresses;
            });
    }
}
