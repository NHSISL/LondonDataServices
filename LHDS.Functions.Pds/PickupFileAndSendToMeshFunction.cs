// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading;
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
        public async Task Run(
            [BlobTrigger("pds/in/{name}", Connection = "BlobStorage")] Stream myBlob,
            string name,
            CancellationToken cancellationToken)
        {
            await this.loggingBroker
                  .LogInformationAsync(
                      $"C# Blob trigger function Processing blob\n " +
                      $"Name: pds/in/{{name}} \n Data: {myBlob}");

            try
            {
                await pdsClient.PickupFileAndSendToMesh(
                    pdsStream: myBlob,
                    fileName: name,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                await this.loggingBroker.LogErrorAsync(ex);
                throw;
            }
        }
    }
}
