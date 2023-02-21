// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace LHDS.Functions.Landings.Emis
{
    public class EmisLandingFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly ILandingClient landingClient;

        public EmisLandingFunction(ILoggingBroker loggingBroker, ILandingClient landingClient)
        {
            this.loggingBroker = loggingBroker;
            this.landingClient = landingClient;
        }

        [Function("EmisLandingFunction")]
        public async ValueTask Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            this.loggingBroker.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                if (landingClient != null)
                {
                    await this.landingClient.ProcessAsync();
                }
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }
    }
}
