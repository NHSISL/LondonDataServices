// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Functions.Addresses.Models;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Landings.Emis
{
    public class AddressSyncToAssignTimerFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IAddressClient addressClient;

        public AddressSyncToAssignTimerFunction(
            ILoggingBroker loggingBroker,
            IAddressClient addressClient)
        {
            this.loggingBroker = loggingBroker;
            this.addressClient = addressClient;
        }

        [Function("AddressSyncToAssignTimerFunction")]
        public async Task Run([TimerTrigger("0 0 * * * *")] MyInformation myTimer)
        {
            this.loggingBroker.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await this.addressClient.SyncAddressesWithAssign();
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
