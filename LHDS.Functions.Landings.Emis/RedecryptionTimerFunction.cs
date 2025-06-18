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
    public class RedecryptionTimerFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IDecryptionClient decryptionClient;
        private readonly ILogger logger;
        private readonly LandingConfiguration landingConfiguration;

        public RedecryptionTimerFunction(
            ILoggingBroker loggingBroker,
            IDecryptionClient decryptionClient,
            ILoggerFactory loggerFactory,
            LandingConfiguration landingConfiguration)
        {
            this.loggingBroker = loggingBroker;
            this.decryptionClient = decryptionClient;
            this.logger = loggerFactory.CreateLogger<EmisLandingTimerFunction>();
            this.landingConfiguration = landingConfiguration;
        }

        [Function("RedecryptionTimerFunction")]
        public async Task Run([TimerTrigger("%RedecryptionTimerInterval%")] MyInfo myTimer)
        {
            await this.loggingBroker.LogInformationAsync($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await this.decryptionClient.RetryDecryptAsync();
            }
            catch (Exception ex)
            {
                await this.loggingBroker.LogErrorAsync(ex);
                throw;
            }

            await this.loggingBroker.LogInformationAsync($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }
}
