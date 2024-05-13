// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NEL.LibPostalClient.Clients;
using NEL.LibPostalClient.Models.Brokers.LibPostal;

namespace LHDS.Core.Brokers.AddressNormalisations
{
    public class AddressNormalisationBroker : IAddressNormalisationBroker
    {
        private readonly ILibPostalClient libPostalClient;

        public AddressNormalisationBroker()
        {
            string assembly = Assembly.GetExecutingAssembly().Location;
            string dataFolderPath = Path.Combine(Path.GetDirectoryName(assembly) ?? string.Empty, @"Data");

            var config = new LibPostalConfiguration
            {
                DataDirectory = dataFolderPath,
                ParserDataDirectory = dataFolderPath,
                LanguageClassifierDataDirectory = dataFolderPath,
                PaserOptions = new ParserOptions
                {
                    Country = "GB",
                    Language = "en",
                }
            };

            libPostalClient = new LibPostalClient(config);
        }

        public async ValueTask<string[]> ExpandAddressAsync(string address) =>
            await libPostalClient.ExpandAddressAsync(address);

        public async ValueTask<List<KeyValuePair<string, string>>> ParseAddressAsync(string address) =>
            await libPostalClient.ParseAddressAsync(address);
    }
}
