// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Extensions.Addresses;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.AddressNormalisations;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationService : IAddressExtractionOrchestrationService
    {
        private readonly IAddressNormalisationService addressNormalisationService;
        private readonly IAuditBroker auditBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;

        public AddressExtractionOrchestrationService(
            IAddressNormalisationService addressNormalisationService,
            IAuditBroker auditBroker,
            ILoggingBroker loggingBroker,
            ICsvHelperBroker csvHelperBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker)
        {
            this.addressNormalisationService = addressNormalisationService;
            this.auditBroker = auditBroker;
            this.loggingBroker = loggingBroker;
            this.csvHelperBroker = csvHelperBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask<List<Address>> ProcessAddressesAsync(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data, filename);

                string stringData = Encoding.UTF8.GetString(data);
                List<string> records = stringData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

                List<string> filteredRecords = records.Where(record =>
                   record.StartsWith("28,") || record.StartsWith("\"28\",")).ToList();

                string stringRecords = string.Join(Environment.NewLine, filteredRecords);

                Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { "UPRN", 3 },
                    { "UPSN", 4 },
                    { "OrganisationName", 5 },
                    { "DepartmentName", 6 },
                    { "SubBuildingName", 7 },
                    { "BuildingName", 8 },
                    { "BuildingNumber", 9 },
                    { "DependentThoroughfare", 10 },
                    { "Thoroughfare", 11 },
                    { "DoubleDependentLocality", 12 },
                    { "DependentLocality", 13 },
                    { "PostTown", 14 },
                    { "PostCode", 15 }
                };

                List<Address> addresses = await this.csvHelperBroker
                    .MapCsvToObjectAsync<Address>(stringRecords, hasHeaderRecord: false, fieldMappings);

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

                        await this.auditBroker.LogInformation(
                            auditType: "Address",
                            title: "Successfully extracted address from Ordinance Database",
                            message: $"Successfully extracted address with id: {address.Id} from file: {filename}",
                            filename,
                            correlationId: address.Id);
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

        public ValueTask<List<ResolvedAddress>> ProcessResolvedAddressesAsync(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data, filename);

                Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { nameof(ResolvedAddress.UniqueReference), 0 },
                    { nameof(ResolvedAddress.PostCode), 1 },
                    { nameof(ResolvedAddress.UnstructuredPostalAddress), 2 }
                };

                List<ResolvedAddress> resolvedAddresses = await this.csvHelperBroker
                    .MapCsvToObjectAsync<ResolvedAddress>(
                        data: Encoding.UTF8.GetString(data),
                        hasHeaderRecord: true,
                        fieldMappings);

                List<ResolvedAddress> processedResolvedAddresses = new List<ResolvedAddress>();
                var exceptions = new List<Exception>();

                foreach (ResolvedAddress resolvedAddress in resolvedAddresses)
                {
                    try
                    {
                        ResolvedAddress processedResolvedAddress = await TryCatch(async () =>
                        {
                            ResolvedAddress inputResolvedAddress = resolvedAddress;
                            inputResolvedAddress.Id = this.identifierBroker.GetIdentifier();
                            string addressString = inputResolvedAddress.UnstructuredPostalAddress;

                            AddressNormalisation addressNormalisation =
                                await this.addressNormalisationService.GetNormalisedAddress(addressString);

                            inputResolvedAddress.JsonPostalAddress = addressNormalisation.JsonPostalAddress;
                            inputResolvedAddress.PostalAddress = addressNormalisation.PostalAddress;

                            await this.auditBroker.LogInformation(
                                auditType: "Address",
                                title: "Successfully extracted address from Ordinance Database",
                                message: $"Successfully extracted address with id: {inputResolvedAddress.Id}" +
                                    $" from file: {filename}",
                                filename,
                                correlationId: resolvedAddress.Id);

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
