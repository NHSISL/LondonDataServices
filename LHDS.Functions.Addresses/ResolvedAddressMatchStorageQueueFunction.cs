// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Core.Models.Brokers.Storages.StorageQueues;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Addresses
{
    public class ResolvedAddressMatchStorageQueueFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IAddressClient addressClient;

        public ResolvedAddressMatchStorageQueueFunction(
            ILoggingBroker loggingBroker,
            IAddressClient addressClient)
        {
            this.loggingBroker = loggingBroker;
            this.addressClient = addressClient;
        }

        [Function("ResolvedAddressMatchStorageQueueFunction")]
        public async Task Run(
            [QueueTrigger("ResolveAddressQueue", Connection = "QueueConnection")] string myQueueItem,
            FunctionContext context)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    WriteIndented = false,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                Payload<Guid> payload =
                    JsonSerializer.Deserialize<Payload<Guid>>(myQueueItem, jsonOptions);

                await this.loggingBroker.LogInformationAsync(
                    $"ResolvedAddressMatchStorageQueueFunction executed at: {DateTime.Now} " +
                    $"for item '{payload?.Message}' queued at '{payload.EnqueuedAtUtc}'");


                await this.addressClient.MatchAddressDataAsync(payload);
            }
            catch (Exception ex)
            {
                await this.loggingBroker.LogErrorAsync(ex);
                throw;
            }
        }
    }
}
