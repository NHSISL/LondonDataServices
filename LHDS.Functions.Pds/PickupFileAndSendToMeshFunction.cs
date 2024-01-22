// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Pds
{
    public class PickupFileAndSendToMeshFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IPdsClient pdsClient;

        public PickupFileAndSendToMeshFunction(
            ILoggingBroker loggingBroker,
            IPdsClient pdsClient)
        {
            this.loggingBroker = loggingBroker;
            this.pdsClient = pdsClient;
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
                throw;
            }
        }
    }
}
