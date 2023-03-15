// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public interface IMeshService
    {
        ValueTask<bool> ValidateMailboxAccessAsync();

        ValueTask<bool> AcknowledgeMessageByIdAsync(string inputMailboxId, string inputMessageId);

        ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync(string mailboxId);

        ValueTask<string> RetrieveMessageByIdAsync(string mailboxId, string messageId);

        ValueTask<string> SendMessageAsync(string messageId);

        ValueTask<string> RetrieveTrackingStatusAsync(string mailboxId, string messageId);
    }
}
