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
    public class ResolvedAddressLoaderFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IAddressClient addressClient;

        public ResolvedAddressLoaderFunction(
            ILoggingBroker loggingBroker,
            IAddressClient addressClient)
        {
            this.loggingBroker = loggingBroker;
            this.addressClient = addressClient;
        }

        [Function("ResolvedAddressLoaderFunction")]
        public async Task Run(
            [BlobTrigger("addresses/resolve/in/{name}", Connection = "BlobStorage")] Stream myBlob, string name)
        {
            await loggingBroker
                  .LogInformationAsync(
                      $"C# Blob trigger function Processing blob\n " +
                      $"Name: address/in/{{name}}");

            try
            {
                await addressClient.LoadAddressesToResolveAsync(data: myBlob, filename: name);
            }
            catch (Exception ex)
            {
                await loggingBroker.LogErrorAsync(ex);
                throw;
            }
        }
    }
}
