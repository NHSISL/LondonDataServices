// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Functions.Terminology.Models;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Terminology
{
    public class TerminologyDetailFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly ITerminologyClient terminologyClient;

        public TerminologyDetailFunction(
            ILoggingBroker loggingBroker,
            ITerminologyClient terminologyClient)
        {
            this.loggingBroker = loggingBroker;
            this.terminologyClient = terminologyClient;
        }

        [Function("TerminologyDetailFunction")]
        public async Task Run([TimerTrigger("0 */15 * * * *")] MyInformation myTimer)
        {
            loggingBroker.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await terminologyClient.RetrieveArtifactDetailsAsync();
            }
            catch (Exception ex)
            {
                loggingBroker.LogError(ex);
                throw;
            }

            loggingBroker.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }
}

