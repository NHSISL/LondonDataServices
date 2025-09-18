// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Azure.Storage.Queues.Models;

namespace LHDS.Core.Brokers.Storages.StorageQueues
{
    internal interface IStorageQueueBroker
    {
        ValueTask SendMessageAsync(string queueName, string message);
        ValueTask<QueueMessage> ReceiveMessageAsync(string queueName);
        ValueTask DeleteMessageAsync(string queueName, string messageId, string popReceipt);
    }
}
