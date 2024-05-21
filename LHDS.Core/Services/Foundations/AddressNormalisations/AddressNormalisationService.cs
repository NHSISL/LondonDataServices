// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LHDS.Core.Brokers.AddressNormalisations;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using Newtonsoft.Json;

namespace LHDS.Core.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationService : IAddressNormalisationService
    {
        private readonly IAddressNormalisationBroker addressNormalisationBroker;
        private readonly ILoggingBroker loggingBroker;

        public AddressNormalisationService(
            IAddressNormalisationBroker addressNormalisationBroker,
            ILoggingBroker loggingBroker)
        {
            this.addressNormalisationBroker = addressNormalisationBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AddressNormalisation> GetNormalisedAddress(string address) =>
            TryCatch(async () =>
            {
                ValidateAddressNormalisationArgs(address);
                string cleanedAddress = CleanupAddress(address);
                string[] expandedAddresses = await this.addressNormalisationBroker.ExpandAddressAsync(address);
                string firstExpandedAddress = expandedAddresses.FirstOrDefault() ?? string.Empty;

                List<KeyValuePair<string, string>> parsedAddress =
                    await this.addressNormalisationBroker.ParseAddressAsync(address: firstExpandedAddress);

                string jsonPostalAddress = ParseAddressToJson(addressParts: parsedAddress);

                AddressNormalisation normalisedAddress = new AddressNormalisation
                {
                    PostalAddress = firstExpandedAddress,
                    JsonPostalAddress = jsonPostalAddress,
                    AddressComponents = parsedAddress,
                };

                return normalisedAddress;
            });

        internal string CleanupAddress(string address)
        {
            //search for line breaks (both Windows and Linux-style) in the
            //string and replace them with a comma followed by a space.
            var cleanAddress = Regex.Replace(address, @"\r\n?|\n", ",");

            // search for consecutive sequences of two or more commas (",{2,}")
            // within the template string and replaces them with a single comma
            cleanAddress = Regex.Replace(cleanAddress, ",{2,}", ",");

            // Ensure every comma is followed by a space
            cleanAddress = Regex.Replace(cleanAddress, @",(?=[^\s])", ", ");

            // search for one or more consecutive whitespace characters in the
            // string and replaces them with a single space.
            cleanAddress = Regex.Replace(cleanAddress, @"\s+", " ");

            // remove empty segments
            cleanAddress = Regex.Replace(cleanAddress, ", (, )+", ", ");

            return cleanAddress;
        }

        internal string ParseAddressToJson(List<KeyValuePair<string, string>> addressParts)
        {
            try
            {
                var dictionary = addressParts.ToDictionary(kv => kv.Key, kv => kv.Value);
                string json = JsonConvert.SerializeObject(dictionary);

                return json;
            }
            catch (Exception ex)
            {
                var address = string.Join(" ", addressParts.Select(pair => pair.Value));

                var addressSegments =
                    string.Join(Environment.NewLine, addressParts.Select(pair => $"{pair.Key}: {pair.Value}"));

                throw new InvalidAddressPartsNormalisationException(
                    $"{ex.Message}, address: {address}, parts: {addressSegments}");
            }
        }
    }
}
