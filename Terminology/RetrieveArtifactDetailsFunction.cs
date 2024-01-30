// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Functions.Terminology.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;

namespace Terminology
{
    public class RetrieveArtifactDetailsFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly ITerminologyClient terminologyClient;

        public RetrieveArtifactDetailsFunction(
            ILoggingBroker loggingBroker,
            ITerminologyClient terminologyClient)
        {
            this.loggingBroker = loggingBroker;
            this.terminologyClient = terminologyClient;
        }

        [FunctionName("RetrieveArtifactDetailsFunction")]
       public async ValueTask Run([TimerTrigger("0 */15 * * * *")] MyInformation myTimer)  
        {  
            this.loggingBroker.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");  

            try  
            {  
                await terminologyClient.RetrieveArtifactDetailsAsync();  
            }  
            catch (Exception ex)  
            {  
                this.loggingBroker.LogError(ex);  
                throw;  
            }  
        }
    }
}
