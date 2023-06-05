// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LHDS.Functions.Pds
{
    public class RetreiveMessagesFromMeshAndUpdateStorage
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IPdsClient pdsClient;
        private readonly ILogger logger;

        public RetreiveMessagesFromMeshAndUpdateStorage(
            ILoggingBroker loggingBroker,
            IPdsClient pdsClient,
            ILoggerFactory loggerFactory)
        {
            this.loggingBroker = loggingBroker;
            this.pdsClient = pdsClient;
            this.logger = loggerFactory.CreateLogger<RetreiveMessagesFromMeshAndUpdateStorage>();
        }

        [Function("RetreiveMessagesFromMeshAndUpdateStorage")]
        public void Run([TimerTrigger("0 */15 * * * *")] MyInfo myTimer)
        {
            this.loggingBroker.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                Task.Run(async () =>
                {
                    await pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();
                }).Wait();
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                this.logger.LogError(ex, ex.Message);
                throw;
            }

            this.loggingBroker.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
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

