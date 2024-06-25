// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Core.Models.Foundations.Documents;
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
            [BlobTrigger("tpplanding/{name}", Connection = "BlobStorage")] string myBlob, string name)
        {
            try
            {
                this.loggingBroker
                    .LogInformation(
                        $"C# Blob trigger function Processing document\n " +
                        $"Name: FileName: {name}");

                using (Stream input = new MemoryStream(Encoding.ASCII.GetBytes(myBlob)))
                {
                    Document document = new Document
                    {
                        FileName = name,
                        DocumentData = input
                    };

                    await tppLandingClient.ProcessAsync(document, supplierId: landingConfiguration.LandingSupplierId);
                }
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }
    }
}