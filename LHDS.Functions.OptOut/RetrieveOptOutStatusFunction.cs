// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.OptOut
{
    public class RetrieveOptOutStatusFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IOptOutClient optOutClient;

        public RetrieveOptOutStatusFunction(
            ILoggingBroker loggingBroker,
            IOptOutClient optOutClient)
        {
            this.loggingBroker = loggingBroker;
            this.optOutClient = optOutClient;
        }

        [Function("RetrieveOptOutStatusFunction")]
        public async Task Run(
            [BlobTrigger("optout/in/{name}", Connection = "BlobStorage")] Stream myBlob, string name)
        {
            await this.loggingBroker
                  .LogInformationAsync(
                      $"C# Blob trigger function Processing blob\n " +
                      $"Name: optout/in/{{name}}");

            try
            {
                await optOutClient.RetrieveOptOutStatusAsync(input: myBlob, fileName: name);
            }
            catch (Exception ex)
            {
                await this.loggingBroker.LogErrorAsync(ex);
                throw;
            }
        }
    }
}
