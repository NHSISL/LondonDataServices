// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Mesh
{
    public interface IMeshBroker
    {
        ValueTask<bool> ValidateAccess();

        ValueTask<bool> AcknowledgeMessageById(string mailboxId, string messageId);

        ValueTask<List<string>> GetMessageIdsFromInbox(string mailboxId);

        ValueTask<string> GetMessageById(string mailboxId, string messageId);

        ValueTask<string> SendMessage(string messageId);

        ValueTask<string> GetTrackingStatus(string mailboxId, string messageId);
    }
}
