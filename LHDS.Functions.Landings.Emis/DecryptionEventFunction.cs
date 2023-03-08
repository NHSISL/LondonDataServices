// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LHDS.Functions.Landings.Emis
{
    public class DecryptionEventFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IDecryptionClient decryptionClient;
        private readonly ILogger logger;

        public DecryptionEventFunction(
            ILoggingBroker loggingBroker,
            IDecryptionClient decryptionClient,
            ILoggerFactory loggerFactory)
        {
            this.loggingBroker = loggingBroker;
            this.decryptionClient = decryptionClient;
            this.logger = loggerFactory.CreateLogger<EmisLandingTimerFunction>();
        }

        [Function("DecryptionEventFunction")]
        public void Run(
            [BlobTrigger("emislanding/encrypted/{name}", Connection = "BlobStorage")] string myBlob, string name)
        {
            this.loggingBroker
                .LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");

            try
            {
                Task.Run(async () =>
                {
                    await this.decryptionClient.DecryptAsync(name);
                }).Wait();
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                this.logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
