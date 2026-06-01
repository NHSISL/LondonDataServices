// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Addresses
{
    public class MatchAddressToUprnFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IAddressToUprnClient addressToUprnClient;

        public MatchAddressToUprnFunction(
            ILoggingBroker loggingBroker,
            IAddressToUprnClient addressToUprnClient)
        {
            this.loggingBroker = loggingBroker;
            this.addressToUprnClient = addressToUprnClient;
        }

        [Function("MatchAddressToUprnFunction")]
        public async Task Run(
            [BlobTrigger("addresstouprn/in/{name}", Connection = "BlobStorage")] Stream myBlob,
            string name,
            CancellationToken cancellationToken = default)
        {
            await this.loggingBroker
                  .LogInformationAsync(
                      $"C# Blob trigger function Processing blob\n " +
                      $"Name: addresstouprn/in/{name}");

            try
            {
                await this.addressToUprnClient.MatchAddressToUprnAsync(
                    data: myBlob,
                    filename: name,
                    correlationId: Guid.NewGuid(),
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                await this.loggingBroker.LogErrorAsync(ex);
                throw;
            }
        }
    }
}
