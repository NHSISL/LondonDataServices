// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net;
using System.Threading.Tasks;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace LHDS.Functions.Landings.Tpp
{
    public class TppLandingFunction
    {
        private readonly ILogger _logger;
        private readonly ILandingClient landingClient;

        public TppLandingFunction(ILoggerFactory loggerFactory, ILandingClient landingClient)
        {
            _logger = loggerFactory.CreateLogger<TppLandingFunction>();
            this.landingClient = landingClient;
        }

        [Function("TppLandingFunction")]
        public async ValueTask<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Processing TPP documents");

            if (landingClient != null)
            {
                await this.landingClient.ProcessAsync();
            }

            return response;
        }
    }
}
