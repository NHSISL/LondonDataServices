// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text;
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
            [BlobTrigger("addresses/ordinance/in/{name}", Connection = "BlobStorage")] string myBlob, string name)
        {
            loggingBroker
                .LogInformation(
                    $"C# Blob trigger function Processing blob\n " +
                    $"Name: pds/in/{{name}} \n Data: {myBlob}");

            try
            {
                byte[] addressData = Encoding.UTF8.GetBytes(myBlob);
                await addressClient.LoadAddressDataAsync(data: addressData, filename: name);
            }
            catch (Exception ex)
            {
                loggingBroker.LogError(ex);
                throw;
            }
        }
    }
}
