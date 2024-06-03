// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Landings.Emis
{
    public class AddressNormalisationTimerFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IAddressClient addressClient;

        public AddressNormalisationTimerFunction(
            ILoggingBroker loggingBroker,
            IAddressClient addressClient)
        {
            this.loggingBroker = loggingBroker;
            this.addressClient = addressClient;
        }

        [Function("AddressNormalisationTimerFunction")]
        public async Task Run([TimerTrigger("0 0 * * * *")] MyInfo myTimer)
        {
            this.loggingBroker.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await this.addressClient.NormaliseAddresses();
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }

            this.loggingBroker.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
