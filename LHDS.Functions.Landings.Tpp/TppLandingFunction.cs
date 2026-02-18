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
            //    bool shouldProcess =
            //        await this.fileNameValidationService.ShouldProcessFileAsync(
            //            fileName: name,
            //            includePattern: landingConfiguration.FileNameIncludePattern,
            //            excludePattern: landingConfiguration.FileNameExcludePattern);

                //string action
                //    = shouldProcess ? "PROCESSING" : "SKIPPING";

                //await this.loggingBroker.LogInformationAsync(
                //    $"C# Blob trigger function {action} document\n " +
                //    $"Name: FileName: {name}");

                //if (!shouldProcess)
                //{
                //    return;
                //}

                await tppLandingClient.ProcessAsync(
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