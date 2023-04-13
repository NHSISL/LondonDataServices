// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
using NEL.MESH.Models.Foundations.Mesh;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public partial class MeshService : IMeshService
    {
        private readonly IMeshBroker meshBroker;
        private readonly ILoggingBroker loggingBroker;

        public MeshService(
            IMeshBroker meshBroker,
            ILoggingBroker loggingBroker)
        {
            this.meshBroker = meshBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<bool> ValidateMailboxAccessAsync() =>
            TryCatch(async () =>
            {
                return await this.meshBroker.HandshakeAsync();
            });

        public ValueTask<Message> SendMessageAsync(Message message) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> SendFileAsync(Message message) =>
            throw new System.NotImplementedException();

        public ValueTask<Message> RetrieveTrackingStatusAsync(string messageId) =>
            throw new NotImplementedException();

        public ValueTask<List<string>> RetrieveMessagesFromInboxAsync() =>
            //TryCatch(async () =>
            //{
            //    return await this.meshBroker.RetrieveMessagesAsync();
            //});
            throw new System.NotImplementedException();

        public ValueTask<Message> RetrieveMessageByIdAsync(string messageId) =>
            //TryCatch(async () =>
            //{
            //    ValidateMessageId(messageId);
            //    return await this.meshBroker.RetrieveMessageAsync(messageId);
            //});
            throw new System.NotImplementedException();

        public ValueTask<bool> AcknowledgeMessageByIdAsync(string inputMessageId) =>
            //TryCatch(async () =>
            //{
            //    ValidateMessageId(inputMessageId);
            //    return await this.meshBroker.AcknowledgeMessageAsync(inputMessageId);
            //});
            throw new System.NotImplementedException();
    }
}
