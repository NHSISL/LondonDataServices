// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Functions.OptOut.Models;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.OptOut
{
    public class ProcessUpdatedOptOutStatusFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IOptOutClient optOutClient;

        public ProcessUpdatedOptOutStatusFunction(
            ILoggingBroker loggingBroker,
            IOptOutClient optOutClient)
        {
            this.loggingBroker = loggingBroker;
            this.optOutClient = optOutClient;
        }

        [Function("ProcessUpdatedOptOutStatusFunction")]
        public async Task Run(
            [TimerTrigger("%ProcessUpdatedOptOutTimerTrigger%")] MyInformation myTimer,
            CancellationToken cancellationToken)
        {
            await this.loggingBroker.LogInformationAsync($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await optOutClient.RetrieveUpdatedMeshConsentStatusesChangesAsync(cancellationToken);
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

