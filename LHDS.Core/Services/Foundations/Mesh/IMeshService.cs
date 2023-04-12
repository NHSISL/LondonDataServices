// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public interface IMeshService
    {
        ValueTask<bool> ValidateMailboxAccessAsync();
        ValueTask<Message> SendMessageAsync(Message message);
        ValueTask<Message> SendFileAsync(Message message);
        ValueTask<Message> RetrieveTrackingStatusAsync(string messageId);
        ValueTask<List<string>> RetrieveMessagesFromInboxAsync();
        ValueTask<Message> RetrieveMessageByIdAsync(string messageId);
        ValueTask<bool> AcknowledgeMessageByIdAsync(string inputMessageId);
    }
}
