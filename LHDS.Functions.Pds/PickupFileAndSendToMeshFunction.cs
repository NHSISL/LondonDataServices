// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LHDS.Functions.Pds
{
    public class PickupFileAndSendToMeshFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IPdsClient pdsClient;
        private readonly ILogger logger;

        public PickupFileAndSendToMeshFunction(
            ILoggingBroker loggingBroker,
            IPdsClient pdsClient,
            ILoggerFactory loggerFactory)
        {
            this.loggingBroker = loggingBroker;
            this.pdsClient = pdsClient;
            this.logger = loggerFactory.CreateLogger<PickupFileAndSendToMeshFunction>();
        }

        [Function("PickupFileAndSendToMeshFunction")]
        public void Run(
            [BlobTrigger("pds/in/{name}", Connection = "BlobStorage")] string myBlob, string name)
        {
            this.loggingBroker
                .LogInformation(
                    $"C# Blob trigger function Processing blob\n " +
                    $"Name: pds/in/{{name}} \n Data: {myBlob}");

            try
            {
                Task.Run(async () =>
                {
                    byte[] pdsFile = Encoding.ASCII.GetBytes(myBlob);
                    await pdsClient.PickupFileAndSendToMesh(pdsFile, fileName: name);
                }).Wait();
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                this.logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
