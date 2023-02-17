// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net;
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
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var query = HttpUtility.ParseQueryString(req.Url.Query);
            var blobName = query["name"];
            logger.LogInformation($"Decrypting document: {blobName}");
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/json; charset=utf-8");

            try
            {
                Task.Run(async () =>
                {
                    await this.decryptionClient.DecryptAsync(blobName);
                }).Wait();
            }
            catch (Exception ex)
            {
                response.WriteString(ex.ToString());
                return response;
            }

            response.WriteString($"Sucessfully Processed: {blobName}", System.Text.Encoding.UTF8);
            return response;
        }
    }
}
