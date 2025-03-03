// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Landings.Tpp
{
    public class TppLandingFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly ITppLandingClient tppLandingClient;
        private readonly LandingConfiguration landingConfiguration;

        public TppLandingFunction(
            ILoggingBroker loggingBroker,
            ITppLandingClient tppLandingClient,
            LandingConfiguration landingConfiguration)
        {
            this.loggingBroker = loggingBroker;
            this.tppLandingClient = tppLandingClient;
            this.landingConfiguration = landingConfiguration;
        }

        [Function("TppLandingFunction")]
        public async Task Run(
            [BlobTrigger("tpplanding/{name}", Connection = "BlobStorage")] Stream myBlob, string name)
        {
            try
            {
                await this.loggingBroker
                      .LogInformationAsync(
                          $"C# Blob trigger function Processing document\n " +
                          $"Name: FileName: {name}");

                await tppLandingClient.ProcessAsync(
                    input: myBlob,
                    fileName: name,
                    supplierId: landingConfiguration.LandingSupplierId);
            }
            catch (Exception ex)
            {
                await this.loggingBroker.LogErrorAsync(ex);
                throw;
            }
        }
    }
}