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
    public class RetreiveMessagesFromMeshAndUpdateStorage
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IPdsClient pdsClient;

        public RetreiveMessagesFromMeshAndUpdateStorage(
            ILoggingBroker loggingBroker,
            IPdsClient pdsClient)
        {
            this.loggingBroker = loggingBroker;
            this.pdsClient = pdsClient;
        }

        [Function("RetreiveMessagesFromMeshAndUpdateStorage")]
        public void Run([TimerTrigger("0 */15 * * * *")] MyInformation myTimer)
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
                throw;
            }

            this.loggingBroker.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }
}

