// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Functions.Landings.Tpp.Models;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Landings.Tpp
{
    public class ReLandTimerFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly ITppLandingClient tppLandingClient;
        private readonly LandingConfiguration landingConfiguration;

        public ReLandTimerFunction(
            ILoggingBroker loggingBroker,
            ITppLandingClient tppLandingClient,
            LandingConfiguration landingConfiguration)
        {
            this.loggingBroker = loggingBroker;
            this.tppLandingClient = tppLandingClient;
            this.landingConfiguration = landingConfiguration;
        }

        [Function("ReLandTimerFunction")]
        public async Task Run([TimerTrigger("%reLandTimerInterval%")] MyInfo myTimer)
        {
            await loggingBroker.LogInformationAsync($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await tppLandingClient.ReProcessAsync(supplierId: landingConfiguration.LandingSupplierId);
            }
            catch (Exception ex)
            {
                await loggingBroker.LogErrorAsync(ex);
                throw;
            }

            await loggingBroker.LogInformationAsync($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }
}
