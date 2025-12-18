// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Brokers.Storages.StorageQueues
{
    using System.Linq;
    using System.Threading.Tasks;
    using Azure.Storage.Queues;
    using Azure.Storage.Queues.Models;
    using LHDS.Core.Brokers.Loggings;

    public class StorageQueueBroker : IStorageQueueBroker
    {
        private readonly QueueServiceClient queueServiceClient;

        public StorageQueueBroker(
            ILoggingBroker loggingBroker,
            QueueServiceClient queueServiceClient)
        {
            this.queueServiceClient = queueServiceClient;
        }

        public async ValueTask SendMessageAsync(string queueName, string message)
        {
            var storageQueueClient = await GetOrCreateQueueAsync(queueName);
            await storageQueueClient.SendMessageAsync(message);
        }

        public async ValueTask<QueueMessage> ReceiveMessageAsync(string queueName)
        {
            var storageQueueClient = await GetOrCreateQueueAsync(queueName);
            QueueMessage[] messages = await storageQueueClient.ReceiveMessagesAsync(maxMessages: 1);

            return messages.FirstOrDefault();
        }

        public async ValueTask DeleteMessageAsync(string queueName, string messageId, string popReceipt)
        {
            var storageQueueClient = await GetOrCreateQueueAsync(queueName);
            await storageQueueClient.DeleteMessageAsync(messageId, popReceipt);
        }

        private async ValueTask<QueueClient> GetOrCreateQueueAsync(string queueName) =>
            queueServiceClient.GetQueueClient(queueName);
    }
}