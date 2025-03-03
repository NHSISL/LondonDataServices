// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Functions.OptOut.Models;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.OptOut
{
    public class PushExpiredOptOutsToMeshForRenewalFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IOptOutClient optOutClient;

        public PushExpiredOptOutsToMeshForRenewalFunction(
            ILoggingBroker loggingBroker,
            IOptOutClient optOutClient)
        {
            this.loggingBroker = loggingBroker;
            this.optOutClient = optOutClient;
        }

        [Function("PushExpiredOptOutsToMeshForRenewalFunction")]
        public async Task Run([TimerTrigger("0 */15 * * * *")] MyInformation myTimer)
        {
            this.loggingBroker.LogInformationAsync($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await optOutClient.PushExpiredOptOutsToMeshForRenewalAsync();
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogErrorAsync(ex);
                throw;
            }

            this.loggingBroker.LogInformationAsync($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }
}
