// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;
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
        public async ValueTask Run(
            [BlobTrigger("emislanding/encrypted/{name}", Connection = "ConnectionString")] string myBlob, string name)
        {
            logger.LogInformation($"Decrypting document: {name}");
            await this.decryptionClient.DecryptAsync(name);
        }
    }
}
