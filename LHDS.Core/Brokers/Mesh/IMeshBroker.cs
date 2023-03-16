// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Mesh
{
    public interface IMeshBroker
    {
        ValueTask<bool> ValidateAccessAsync();

        ValueTask<bool> AcknowledgeMessageByIdAsync(string mailboxId, string messageId);

        ValueTask<List<string>> GetMessageIdsFromInboxAsync(string mailboxId);

        ValueTask<string> GetMessageByIdAsync(string mailboxId, string messageId);

        ValueTask<string> SendMessageAsync(string messageId);

        ValueTask<string> GetTrackingStatusAsync(string mailboxId, string messageId);
    }
}
