// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Functions.IDecide.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LHDS.Functions.IDecide
{
    public class GetPatientDecisionsFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IIDecideClient iDecideClient;
        private readonly ILogger logger;

        public GetPatientDecisionsFunction(
            ILoggingBroker loggingBroker,
            IIDecideClient iDecideClient,
            ILoggerFactory loggerFactory)
        {
            this.loggingBroker = loggingBroker;
            this.iDecideClient = iDecideClient;
            this.logger = loggerFactory.CreateLogger<GetPatientDecisionsFunction>();
        }

        [Function("GetPatientDecisionsFunction")]
        public async Task Run([TimerTrigger("%getPatientDecisionsFunction%")] MyInformation myTimer)
        {
            await this.loggingBroker.LogInformationAsync($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await iDecideClient.GetPatientDecisions();
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
