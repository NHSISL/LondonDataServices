// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Core.Models.Foundations.Documents;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Landings.Tpp
{
    public class TppLandingFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly ITppLandingClient tppLandingClient;

        public TppLandingFunction(
            ILoggingBroker loggingBroker,
            ITppLandingClient tppLandingClient)
        {
            this.loggingBroker = loggingBroker;
            this.tppLandingClient = tppLandingClient;
        }

        [Function("TppLandingFunction")]
        public async Task Run(
            [BlobTrigger("tpplanding", Connection = "BlobStorage")] Document document)
        {
            this.loggingBroker
                .LogInformation(
                    $"C# Blob trigger function Processing document\n " +
                    $"Name: FileName: {document.FileName}");

            try
            {
                await tppLandingClient.ProcessAsync(document);
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }
    }
}