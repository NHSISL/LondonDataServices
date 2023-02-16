// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web;
using LHDS.Core.Clients;
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
        public async ValueTask Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var query = HttpUtility.ParseQueryString(req.Url.Query);
            var blobName = query["name"];
            logger.LogInformation($"Decrypting document: {blobName}");
            if(blobName != null)
            {
                await this.decryptionClient.DecryptAsync(blobName);
            }
        }
    }
}
