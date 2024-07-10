// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Functions.Addresses.Models;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Addresses
{
    public class ResolvedAddressMatchTimerFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IAddressClient addressClient;

        public ResolvedAddressMatchTimerFunction(
            ILoggingBroker loggingBroker,
            IAddressClient addressClient)
        {
            this.loggingBroker = loggingBroker;
            this.addressClient = addressClient;
        }

        [Function("ResolvedAddressMatchTimerFunction")]
        public async Task Run([TimerTrigger("0 0 * * * *")] MyInformation myTimer)
        {
            loggingBroker.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await addressClient.MatchAddressDataAsync();
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
