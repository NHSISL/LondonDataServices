// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Functions.Pds.Models;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Pds
{
    public class HandShakeFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IPdsClient pdsClient;

        public HandShakeFunction(
            ILoggingBroker loggingBroker,
            IPdsClient pdsClient)
        {
            this.loggingBroker = loggingBroker;
            this.pdsClient = pdsClient;
        }

        [Function("HandShakeFunction")]
        public async Task Run([TimerTrigger("0 0 23 * * *")] MyInformation myTimer)
        {
            this.loggingBroker.LogInformationAsync($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await pdsClient.ValidateMailboxAccessAsync();
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
