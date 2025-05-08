// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Addresses
{
    public class AddressLoaderFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IAddressClient addressClient;

        public AddressLoaderFunction(
            ILoggingBroker loggingBroker,
            IAddressClient addressClient)
        {
            this.loggingBroker = loggingBroker;
            this.addressClient = addressClient;
        }

        [Function("AddressLoaderFunction")]
        public async Task Run(
            [BlobTrigger("addresses/ordinance/in/{name}", Connection = "BlobStorage")] Stream myBlob, string name)
        {
            await loggingBroker
                  .LogInformationAsync(
                     $"C# Blob trigger function Processing blob\n " +
                     $"Name: addresses/ordinance/in/{{name}}");

            try
            {
                await addressClient.LoadAddressDataAsync(data: myBlob, filename: name);
            }
            catch (Exception ex)
            {
                await loggingBroker.LogErrorAsync(ex);
                throw;
            }
        }
    }
}
