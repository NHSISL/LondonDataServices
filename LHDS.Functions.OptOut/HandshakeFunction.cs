// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
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
        public void Run([TimerTrigger("0 0 0 * * *")] MyInfo myTimer)
        {
            this.loggingBroker.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                Task.Run(async () =>
                {
                    await optOutClient.PushExpiredOptOutsToMeshForRenewalAsync();
                }).Wait();
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }

            this.loggingBroker.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }

    public class MyInformation
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatusDetails
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}

