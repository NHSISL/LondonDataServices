// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Functions.Landings.Emis.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LHDS.Functions.Landings.Emis
{
    public class EmisLandingTimerFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IEmisLandingClient landingClient;
        private readonly ILogger logger;
        private readonly LandingConfiguration landingConfiguration;

        public EmisLandingTimerFunction(
            ILoggingBroker loggingBroker,
            IEmisLandingClient landingClient,
            ILoggerFactory loggerFactory,
            LandingConfiguration landingConfiguration)
        {
            this.loggingBroker = loggingBroker;
            this.landingClient = landingClient;
            this.logger = loggerFactory.CreateLogger<EmisLandingTimerFunction>();
            this.landingConfiguration = landingConfiguration;
        }

        [Function("EmisLandingTimerFunction")]
        public async Task Run([TimerTrigger("0 */15 * * * *")] MyInfo myTimer)
        {
            this.loggingBroker.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await this.landingClient.ProcessAsync(supplierId: landingConfiguration.LandingSupplierId);
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }

            this.loggingBroker.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }
}
