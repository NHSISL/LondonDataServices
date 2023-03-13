// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Mesh
{
    public class MeshBroker : IMeshBroker
    {
        public ValueTask<bool> AcknowledgeMessageById(string mailboxId, string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<string> GetMessageById(string mailboxId, string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> GetMessageIdsFromInbox(string mailboxId) =>
            throw new System.NotImplementedException();

        public ValueTask<string> GetTrackingStatus(string mailboxId, string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<string> SendMessage(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<bool> ValidateAccess() =>
            throw new System.NotImplementedException();
    }
}
