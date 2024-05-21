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
                            //check if the address exists -> retrieve all matching on given fields
                            //if not exists -> generated a guid and add to address
                            //if exists -> add guid 

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
                 AddressComponents = GenerateKeyValuePairAddressFromJson(address.JsonPostalAddress)
             }).ToHashSet();

             AddressMatch matchedAddress =
                 await addressMatcherProcessingService.FindBestMatch(
                     matchedAddresses: addressesToMatch,
                     addressComponents: resolvedAddressComponents);

             ResolvedAddress UpdateFromMatch = populateMatchedAddress(resolvedAddresses, matchedAddress);
             UpdateFromMatch.UpdatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset();

             ResolvedAddress updatedResolvedAddress =
                 await resolvedAddressProcessingService.ModifyResolvedAddressAsync(UpdateFromMatch);

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
            Enum.TryParse(((int)matchedAddress.BestMatch).ToString(), ignoreCase: true, out matchAlgorithmEnum);
            resolvedAddresses.MatchAlgorithmEnum = matchAlgorithmEnum;
            resolvedAddresses.IsMatched = matchedAddress.IsMatched;
            resolvedAddresses.MatchedWithPostalAddress = matchedAddress.PostalAddress;
            resolvedAddresses.MatchedWithJsonPostalAddress = matchedAddress.JsonPostalAddress;
            resolvedAddresses.MatchedUPRN = matchedAddress.UPRN;
            resolvedAddresses.MatchedUPSN = matchedAddress.UPSN;

            if (matchedAddress.AddressComponents.Count() > 0)
            {
                resolvedAddresses.MatchedOrganisationName =
                matchedAddress.AddressComponents.FirstOrDefault(pair => pair.Key == "OrganisationName").Value;

                resolvedAddresses.MatchedDepartmentName =
                    matchedAddress.AddressComponents.FirstOrDefault(pair => pair.Key == "MatchedDepartmentName").Value;

                resolvedAddresses.MatchedSubBuildingName =
                    matchedAddress.AddressComponents.FirstOrDefault(pair => pair.Key == "MatchedSubBuildingName").Value;

                resolvedAddresses.MatchedBuildingName =
                    matchedAddress.AddressComponents.FirstOrDefault(pair => pair.Key == "MatchedBuildingName").Value;

                resolvedAddresses.MatchedBuildingNumber =
                    matchedAddress.AddressComponents.FirstOrDefault(pair => pair.Key == "MatchedBuildingNumber").Value;

                resolvedAddresses.MatchedDependentThoroughfare =
                    matchedAddress.AddressComponents
                        .FirstOrDefault(pair => pair.Key == "MatchedDependentThoroughfare").Value;

                resolvedAddresses.MatchedThoroughfare =
                    matchedAddress.AddressComponents.FirstOrDefault(pair => pair.Key == "MatchedThoroughfare").Value;

                resolvedAddresses.MatchedDoubleDependentLocality =
                    matchedAddress.AddressComponents
                        .FirstOrDefault(pair => pair.Key == "MatchedDoubleDependentLocality").Value;

                resolvedAddresses.MatchedDependentLocality =
                    matchedAddress.AddressComponents.FirstOrDefault(pair => pair.Key == "MatchedDependentLocality").Value;

                resolvedAddresses.MatchedPostTown =
                    matchedAddress.AddressComponents.FirstOrDefault(pair => pair.Key == "MatchedPostTown").Value;

                resolvedAddresses.MatchedPostCode =
                    matchedAddress.AddressComponents.FirstOrDefault(pair => pair.Key == "MatchedPostCode").Value;
            }

            return resolvedAddresses;
        }

        public static List<KeyValuePair<string, string>> GenerateKeyValuePairAddressFromJson(string? jsonPostalAddress)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            var items = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonPostalAddress ?? "");

            if (items == null || items.Count == 0)
            {
                return keyValuePairs;
            }

            foreach (var item in items)
            {
                var key = item["Key"];
                var value = item["Value"];
                keyValuePairs.Add(new KeyValuePair<string, string>(key, value));
            }

            return keyValuePairs;
        }

        public static string ConvertToJSONString(List<KeyValuePair<string, string>> keyValuePairs)
        {
            var jsonString = System.Text.Json.JsonSerializer.Serialize(keyValuePairs);
            return jsonString;
        }
    }
}
