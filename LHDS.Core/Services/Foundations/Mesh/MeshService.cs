// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Models.Foundations.Mesh;

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

        public ValueTask<MeshMessage> SendMessageAsync(MeshMessage message) =>
            throw new System.NotImplementedException();

        public ValueTask<MeshMessage> SendFileAsync(MeshMessage message) =>
            throw new System.NotImplementedException();

        public ValueTask<MeshMessage> RetrieveTrackingStatusAsync(string messageId) =>
            throw new NotImplementedException();

        public ValueTask<List<string>> RetrieveMessagesFromInboxAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<MeshMessage> RetrieveMessageByIdAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<bool> AcknowledgeMessageByIdAsync(string inputMessageId) =>
            throw new System.NotImplementedException();
    }
}
