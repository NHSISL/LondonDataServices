// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Processings.Mesh
{
    public interface IMeshProcessingService
    {
        ValueTask<bool> ValidateMailboxAccessAsync();
        ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync(string mailboxId);
        ValueTask<string> RetrieveAndAcknowledgeMessageByIdAsync(string mailboxId, string messageId);
        ValueTask<string> SendMessageAsync(string mailboxId, string messageId);
    }
}
