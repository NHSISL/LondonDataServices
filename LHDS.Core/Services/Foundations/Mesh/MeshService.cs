// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;

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
                return await this.meshBroker.ValidateAccessAsync();
            });

        public ValueTask<bool> AcknowledgeMessageByIdAsync(string inputMailboxId, string inputMessageId) =>
            TryCatch(async () =>
            {
                ValidateMeshArgs(inputMailboxId, inputMessageId);
                return await this.meshBroker.AcknowledgeMessageByIdAsync(inputMailboxId, inputMessageId);
            });

        public ValueTask<string> RetrieveMessageByIdAsync(string mailboxId, string messageId) =>
            TryCatch(async () =>
            {
                ValidateMeshArgs(mailboxId, messageId);
                return await this.meshBroker.GetMessageByIdAsync(mailboxId, messageId);
            });

        public ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync(string mailboxId) =>
            throw new System.NotImplementedException();

        public ValueTask<string> SendMessageAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateMessageId(messageId);
                return await this.meshBroker.SendMessageAsync(messageId);
            });

        public ValueTask<string> RetrieveTrackingStatusAsync(string mailboxId, string messageId) =>
            TryCatch(async () =>
            {
                ValidateMeshArgs(mailboxId, messageId);
                return await this.meshBroker.GetTrackingStatusAsync(mailboxId, messageId);
            });
    }
}
