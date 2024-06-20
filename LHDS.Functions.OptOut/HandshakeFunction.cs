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
    public class HandshakeFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IOptOutClient optOutClient;

        public HandshakeFunction(
            ILoggingBroker loggingBroker,
            IOptOutClient optOutClient)
        {
            this.loggingBroker = loggingBroker;
            this.optOutClient = optOutClient;
        }

        [Function("HandshakeFunction")]
        public async Task Run([TimerTrigger("0 0 23 * * *")] MyInformation myTimer)
        {
            this.loggingBroker.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await optOutClient.ValidateMailboxAccessAsync();
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

