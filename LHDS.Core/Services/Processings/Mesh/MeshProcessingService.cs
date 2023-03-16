// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
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

        public ValueTask<string> RetrieveAndAcknowledgeMessageByIdAsync(string mailboxId, string messageId) =>
            throw new NotImplementedException();

        public ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync(string mailboxId) =>
            throw new NotImplementedException();

        public ValueTask<string> SendMessageAsync(string mailboxId, string messageId) =>
            throw new NotImplementedException();

        public ValueTask<bool> ValidateMailboxAccessAsync() =>
        TryCatch(async () =>
            {
                return await this.meshService.ValidateMailboxAccessAsync();
            });
    }
}
