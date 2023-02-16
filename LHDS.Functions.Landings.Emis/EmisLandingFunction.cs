// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net;
using System.Threading.Tasks;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace LHDS.Functions.Landings.Emis
{
    public class EmisLandingFunction
    {
        private readonly ILogger _logger;
        private readonly ILandingClient landingClient;

        public EmisLandingFunction(ILoggerFactory loggerFactory, ILandingClient landingClient)
        {
            _logger = loggerFactory.CreateLogger<EmisLandingFunction>();
            this.landingClient = landingClient;
        }
        
        [Function("EmisLandingFunction")]
        public async ValueTask<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Processing EMIS documents");

            if (landingClient != null)
            {
                await this.landingClient.ProcessAsync();
            }

            return response;
        }
    }
}
