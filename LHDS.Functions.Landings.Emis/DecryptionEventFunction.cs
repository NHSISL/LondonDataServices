// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Landings.Emis
{
    public class DecryptionEventFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IDecryptionClient decryptionClient;

        public DecryptionEventFunction(
            ILoggingBroker loggingBroker,
            IDecryptionClient decryptionClient)
        {
            this.loggingBroker = loggingBroker;
            this.decryptionClient = decryptionClient;
        }

        [Function("DecryptionEventFunction")]
        public async Task Run(
            [BlobTrigger("emislanding/encrypted/{name}", Connection = "BlobStorage")] string myBlob, string name)
        {
            await this.loggingBroker
                .LogInformationAsync(
                    $"C# Blob trigger function Processing blob\n " +
                    $"Name: emislanding/encrypted/{name}");

            try
            {
                if (!Path.HasExtension(name))
                {
                    return;
                }

                if (Path.GetExtension(name) == ".gpg")
                {
                    await this.decryptionClient.DecryptAsync($"/encrypted/{name}");
                }
            }
            catch (Exception ex)
            {
                await this.loggingBroker.LogErrorAsync(ex);
                throw;
            }
        }
    }
}
