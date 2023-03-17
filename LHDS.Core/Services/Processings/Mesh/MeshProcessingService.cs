// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.Mesh;

namespace LHDS.Core.Services.Processings.Mesh
{
    public partial class MeshProcessingService : IMeshProcessingService
    {
        private readonly IMeshService meshService;
        private readonly ILoggingBroker loggingBroker;

        public MeshProcessingService(
            IMeshService meshService,
            ILoggingBroker loggingBroker)
        {
            this.meshService = meshService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<bool> ValidateMailboxAccessAsync() =>
           TryCatch(async () =>
           {
               return await this.meshService.ValidateMailboxAccessAsync();
           });

        public ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync(string mailboxId) =>
            TryCatch(async () =>
            {
                ValidateGetArguments(mailboxId);
                return await this.meshService.RetrieveMessageIdsFromInboxAsync(mailboxId);
            });

        public ValueTask<string> RetrieveAndAcknowledgeMessageByIdAsync(string mailboxId, string messageId) =>
         TryCatch(async () =>
            {
                ValidateMeshArgs(mailboxId, messageId);
                var retrievedMessage = await this.meshService.RetrieveMessageByIdAsync(mailboxId, messageId);
                await this.meshService.AcknowledgeMessageByIdAsync(mailboxId, messageId);

                return retrievedMessage;
            });

        public async ValueTask<string> SendMessageAsync(string mailboxId, string messageId)
        {
            var trackMessage = await this.meshService.RetrieveTrackingStatusAsync(mailboxId, messageId);
            await this.meshService.SendMessageAsync(messageId);

            return trackMessage;
        }
    }
}
