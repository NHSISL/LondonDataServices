// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.AddressNormalisations;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationService : IAddressExtractionOrchestrationService
    {
        private readonly IAddressNormalisationProcessingService addressNormalisationProcessingService;
        private readonly IAddressProcessingService addressProcessingService;
        private readonly IAuditBroker auditBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;

        public AddressExtractionOrchestrationService(
            IAddressNormalisationProcessingService addressNormalisationProcessingService,
            IAddressProcessingService addressProcessingService,
            IAuditBroker auditBroker,
            ILoggingBroker loggingBroker,
            ICsvHelperBroker csvHelperBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker)
        {
            this.addressNormalisationProcessingService = addressNormalisationProcessingService;
            this.addressProcessingService = addressProcessingService;
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
                List<byte[]> csvData = await ProcessAddressDataAsync(data);
                List<Address> mappedAddresses = await CsvToAddressAsync(csvData);
                var exceptions = new List<Exception>();

                foreach (var incomingAddress in mappedAddresses)
                {
                    try
                    {
                        Address addressToProcess = incomingAddress;

                        addressToProcess = await TryCatch(async () =>
                        {
                            string addressString = addressToProcess.GetFormattedAddress();

                            AddressNormalisation addressNormalisation =
                                await this.addressNormalisationProcessingService.GetNormalisedAddress(addressString);

                            addressToProcess.JsonPostalAddress = addressNormalisation.JsonPostalAddress;
                            addressToProcess.PostalAddress = addressNormalisation.PostalAddress;
                            addressToProcess.IsErrored = false;

                            return addressToProcess;
                        });
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.InnerException is InvalidAddressPartsNormalisationException)
                        {
                            incomingAddress.IsErrored = true;

                            await this.auditBroker.LogWarning(
                                auditType: "Address",
                                title: "Invalid address parts found",
                                message: $"Invalid address parts found in address with UPRN: {incomingAddress.UPRN} " +
                                    $"from file: {filename}" + Environment.NewLine +
                                    $"error message: {ex.InnerException.Message}" + Environment.NewLine +
                                    $"parts: {ex.InnerException.InnerException.Message}",
                                filename,
                                correlationId: null);
                        }
                        else
                        {
                            exceptions.Add(ex);
                        }
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to normalise address for {exceptions.Count} addresses",
                        exceptions);
                }

                await this.addressProcessingService
                    .BulkAddAddressesAsync(addresses: mappedAddresses, fileName: filename);

                return mappedAddresses;
            });

        private async ValueTask<List<Address>> CsvToAddressAsync(List<byte[]> csvData)
        {
            List<Address> mappedAddresses = new List<Address>();

            foreach (byte[] file in csvData)
            {
                string stringData = Encoding.UTF8.GetString(file);
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

                mappedAddresses.AddRange(addresses);
            }

            return mappedAddresses;
        }

        public ValueTask<List<ResolvedAddress>> ProcessResolvedAddressesAsync(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data, filename);

                Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { nameof(ResolvedAddress.UniqueReference), 0 },
                    { nameof(ResolvedAddress.PostCode), 1 },
                    { nameof(ResolvedAddress.PostalAddress), 2 }
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
                            string addressString = inputResolvedAddress.PostalAddress ?? string.Empty;

                            AddressNormalisation addressNormalisation =
                                await this.addressNormalisationProcessingService.GetNormalisedAddress(addressString);

                            inputResolvedAddress.JsonPostalAddress = addressNormalisation.JsonPostalAddress;

                            inputResolvedAddress.UnstructuredPostalAddress =
                                addressNormalisation.PostalAddress ?? string.Empty;

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

        private async ValueTask<List<byte[]>> ProcessAddressDataAsync(byte[] data)
        {
            List<byte[]> result = new List<byte[]>();
            using (MemoryStream memoryStream = new MemoryStream(data))
            using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.Name.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    {
                        using (Stream entryStream = entry.Open())
                        using (MemoryStream tempMemoryStream = new MemoryStream())
                        {
                            await entryStream.CopyToAsync(tempMemoryStream);
                            byte[] csvData = tempMemoryStream.ToArray();
                            result.Add(csvData);
                        }
                    }
                    else if (entry.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        using (Stream entryStream = entry.Open())
                        using (MemoryStream nestedMemoryStream = new MemoryStream())
                        {
                            await entryStream.CopyToAsync(nestedMemoryStream);
                            byte[] nestedZipData = nestedMemoryStream.ToArray();
                            var nestedResults = await ProcessAddressDataAsync(nestedZipData);
                            result.AddRange(nestedResults);
                        }
                    }
                }
            }

            return result;
        }
    }
}
