// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LHDS.Functions.Landings.Emis
{
    public class EmisLandingTimerFunction
    {
        private readonly ILogger logger;
        private readonly ILandingClient landingClient;

        public EmisLandingTimerFunction(ILoggerFactory loggerFactory, ILandingClient landingClient)
        {
            this.logger = loggerFactory.CreateLogger<EmisLandingTimerFunction>();
            this.landingClient = landingClient;
        }

        [Function("EmisLandingTimerFunction")]
        public void Run([TimerTrigger("0 */15 * * * *")] MyInfo myTimer)
        {
            this.logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                Task.Run(async () =>
                {
                    await this.landingClient.ProcessAsync();
                }).Wait();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw;
            }

            this.logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
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
