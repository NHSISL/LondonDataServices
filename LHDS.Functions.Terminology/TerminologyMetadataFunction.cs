// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Functions.Terminology.Models;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Terminology
{
    public class TerminologyMetadataFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly ITerminologyClient terminologyClient;

        public TerminologyMetadataFunction(
            ILoggingBroker loggingBroker,
            ITerminologyClient terminologyClient)
        {
            this.loggingBroker = loggingBroker;
            this.terminologyClient = terminologyClient;
        }

        [Function("TerminologyMetadataFunction")]
        public async Task Run([TimerTrigger("%TerminologyMetadataInterval%")] MyInformation myTimer)
        {
            await loggingBroker.LogInformationAsync($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                await terminologyClient.RetrieveArtifactMetadataAsync(["CodeSystem", "ValueSet", "ConceptMap"]);
            }
            catch (Exception ex)
            {
                await loggingBroker.LogErrorAsync(ex);
                throw;
            }

            await loggingBroker.LogInformationAsync($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }
}
