// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private readonly IAuditBroker auditBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AddressPersistanceOrchestrationService(
            IAddressProcessingService addressProcessingService,
            IAddressMatcherProcessingService addressMatcherProcessingService,
            IResolvedAddressProcessingService resolvedAddressProcessingService,
            IAuditBroker auditBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.addressProcessingService = addressProcessingService;
            this.addressMatcherProcessingService = addressMatcherProcessingService;
            this.resolvedAddressProcessingService = resolvedAddressProcessingService;
            this.auditBroker = auditBroker;
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

                        processedAddresses.Add(processAddress);

                        await this.auditBroker.LogInformation(
                            auditType: "Address",
                            title: "Successfully persisted Address to Database",
                            message: $"Successfully persisted address with id: {address.Id} from file: {fileName}",
                            fileName,
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
                        $"Unable to add or modify {exceptions.Count} address(es)",
                        exceptions);
                }

                return processedAddresses;
            });

        public ValueTask<ResolvedAddress> MatchAndPersistResolvedAddressAsync(ResolvedAddress resolvedAddresses) =>
         TryCatch(async () =>
         {
             ValidateResolvedAddress(resolvedAddresses);
             ValidatePostCode(resolvedAddresses.PostCode);
             ValidateJsonPostalAddress(resolvedAddresses.JsonPostalAddress);

             string postCode = addressMatcherProcessingService.ExtractPostCode(resolvedAddresses.PostalAddress);
             ValidatePostCode(postCode);
             ValidatPostCodeMatch(resolvedAddresses.PostCode, postCode);

             List<Address> retrieveAddressesByPostCode =
                 await addressProcessingService.RetrieveAddressesByPostCodeAsync(postCode);

             List<KeyValuePair<string, string>> resolvedAddressComponents =
                GenerateKeyValuePairAddressFromJson(resolvedAddresses.JsonPostalAddress);

             HashSet<AddressMatch> addressesToMatch = retrieveAddressesByPostCode.Select(address => new AddressMatch
             {
                 PostalAddress = address.PostalAddress,
                 JsonPostalAddress = address.JsonPostalAddress,
                 NormalisedAddressComponents = GenerateKeyValuePairAddressFromJson(address.JsonPostalAddress),
                 OriginalAddressComponents = GenerateKeyValuePairRepresentingAddress(address),
             }).ToHashSet();

             AddressMatch matchedAddress =
                 await addressMatcherProcessingService.FindBestMatch(
                     matchedAddresses: addressesToMatch,
                     addressComponents: resolvedAddressComponents);

             ResolvedAddress UpdateFromMatch = populateMatchedAddress(resolvedAddresses, matchedAddress);
             DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
             UpdateFromMatch.UpdatedDate = currentDateTime;
             UpdateFromMatch.CreatedDate = currentDateTime;
             UpdateFromMatch.CreatedBy = "System";
             UpdateFromMatch.UpdatedBy = "System";

             ResolvedAddress updatedResolvedAddress =
                 await resolvedAddressProcessingService.ModifyOrAddResolvedAddressAsync(UpdateFromMatch);

             await this.auditBroker.LogInformation(
                 "Resolved Address",
                 "Successfully resolved and address to the database",
                 $"Successfully persisted address with id: " +
                     $"{updatedResolvedAddress.Id} with a {updatedResolvedAddress.MatchAlgorithmEnum} match",
                 updatedResolvedAddress.MatchAlgorithmEnum.ToString(),
                 updatedResolvedAddress.Id);

             return updatedResolvedAddress;
         });

        private static ResolvedAddress populateMatchedAddress(
            ResolvedAddress resolvedAddresses,
            AddressMatch matchedAddress)
        {
            MatchAlgorithmEnum matchAlgorithmEnum = MatchAlgorithmEnum.Human;
            resolvedAddresses.IsMatched = false;

            if (matchedAddress != null)
            {
                Enum.TryParse(((int)matchedAddress.BestMatch).ToString(), ignoreCase: true, out matchAlgorithmEnum);
                resolvedAddresses.MatchAlgorithmEnum = matchAlgorithmEnum;
                resolvedAddresses.IsMatched = matchedAddress.IsMatched;

                if (matchedAddress.OriginalAddressComponents.Count() > 0)
                {
                    resolvedAddresses.MatchedPostalAddress = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "PostalAddress").Value;

                    resolvedAddresses.MatchedJsonPostalAddress = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "JsonPostalAddress").Value;

                    resolvedAddresses.MatchedUPRN = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "UPRN").Value;

                    resolvedAddresses.MatchedUPSN = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "UPSN").Value;

                    resolvedAddresses.MatchedOrganisationName = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "OrganisationName").Value;

                    resolvedAddresses.MatchedDepartmentName = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "DepartmentName").Value;

                    resolvedAddresses.MatchedSubBuildingName = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "SubBuildingName").Value;

                    resolvedAddresses.MatchedBuildingName = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "BuildingName").Value;

                    resolvedAddresses.MatchedBuildingNumber = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "BuildingNumber").Value;

                    resolvedAddresses.MatchedDependentThoroughfare = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "DependentThoroughfare").Value;

                    resolvedAddresses.MatchedThoroughfare = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "Thoroughfare").Value;

                    resolvedAddresses.MatchedDoubleDependentLocality = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "DoubleDependentLocality").Value;

                    resolvedAddresses.MatchedDependentLocality = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "DependentLocality").Value;

                    resolvedAddresses.MatchedPostTown = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "PostTown").Value;

                    resolvedAddresses.MatchedPostCode = matchedAddress.OriginalAddressComponents
                        .FirstOrDefault(pair => pair.Key == "PostCode").Value;
                }
            }

            return resolvedAddresses;
        }

        private static List<KeyValuePair<string, string>> GenerateKeyValuePairRepresentingAddress(Address address)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            PropertyInfo[] properties = typeof(Address).GetProperties();

            foreach (var property in properties)
            {
                object? value = property.GetValue(address);
                string stringValue = value?.ToString() ?? string.Empty;
                keyValuePairs.Add(new KeyValuePair<string, string>(property.Name, stringValue));
            }

            return keyValuePairs;
        }

        private static List<KeyValuePair<string, string>> GenerateKeyValuePairAddressFromJson(string? jsonPostalAddress)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();

            if (string.IsNullOrWhiteSpace(jsonPostalAddress))
            {
                return keyValuePairs;
            }

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPostalAddress);

            foreach (var item in dictionary)
            {
                var key = item.Key;
                var value = item.Value;
                keyValuePairs.Add(new KeyValuePair<string, string>(key, value));
            }

            return keyValuePairs;
        }
    }
}
