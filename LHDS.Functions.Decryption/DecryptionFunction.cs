// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Landings.Client.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace LHDS.Functions.Decryption
{
    public class DecryptionFunction
    {
        private readonly ILogger logger;
        private readonly IDecryptionClient decryptionClient;

        public DecryptionFunction(ILoggerFactory loggerFactory, IDecryptionClient decryptionClient)
        {
            logger = loggerFactory.CreateLogger<DecryptionFunction>();
            this.decryptionClient = decryptionClient;
        }

        [Function("DecryptionFunction")]
        public async ValueTask Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            //logger.LogInformation($"Decrypting document: {name}");

            //TODO: Change to blob trigger 
            //await this.decryptionClient.DecryptAsync(nameame);

        }
    }
}
