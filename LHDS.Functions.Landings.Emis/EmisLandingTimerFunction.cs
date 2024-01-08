// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RESTFulSense.Clients;

namespace LHDS.Functions.Landings.Emis
{
    public class EmisLandingTimerFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly ILandingClient landingClient;
        private readonly ILogger logger;

        public EmisLandingTimerFunction(
            ILoggingBroker loggingBroker,
            ILandingClient landingClient,
            ILoggerFactory loggerFactory)
        {
            this.loggingBroker = loggingBroker;
            this.landingClient = landingClient;
            this.logger = loggerFactory.CreateLogger<EmisLandingTimerFunction>();
        }

        [Function("EmisLandingTimerFunction")]
        public void Run([TimerTrigger("0 0 */3 * * *")] MyInfo myTimer)
        {

            this.loggingBroker.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                Task.Run(async () =>
                {
                    await this.landingClient.ProcessAsync();
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
