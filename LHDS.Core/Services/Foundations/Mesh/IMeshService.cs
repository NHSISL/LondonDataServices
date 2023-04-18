// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public interface IMeshService
    {
        ValueTask<bool> ValidateMailboxAccessAsync();
        ValueTask<MeshMessage> SendMessageAsync(MeshMessage message);
        ValueTask<MeshMessage> SendFileAsync(MeshMessage message);
        ValueTask<MeshMessage> RetrieveTrackingStatusByIdAsync(string messageId);
        ValueTask<List<string>> RetrieveMessagesFromInboxAsync();
        ValueTask<MeshMessage> RetrieveMessageByIdAsync(string messageId);
        ValueTask<bool> AcknowledgeMessageByIdAsync(string inputMessageId);
    }
}
