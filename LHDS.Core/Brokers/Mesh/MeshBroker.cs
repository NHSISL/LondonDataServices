// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Mesh
{
    public class MeshBroker : IMeshBroker
    {
        public ValueTask<bool> AcknowledgeMessageByIdAsync(string mailboxId, string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<string> GetMessageByIdAsync(string mailboxId, string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> GetMessageIdsFromInboxAsync(string mailboxId) =>
            throw new System.NotImplementedException();

        public ValueTask<string> GetTrackingStatusAsync(string mailboxId, string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<string> SendMessageAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<bool> ValidateAccessAsync() =>
            throw new System.NotImplementedException();
    }
}
