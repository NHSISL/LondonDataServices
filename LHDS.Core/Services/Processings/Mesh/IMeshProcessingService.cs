// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;

namespace LHDS.Core.Services.Processings.Mesh
{
    public interface IMeshProcessingService
    {
        ValueTask<bool> ValidateMailboxAccessAsync();
        ValueTask<MeshMessage> SendMessageAsync(MeshMessage message);
        ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync();
        ValueTask<MeshMessage> RetrieveAndAcknowledgeMessageByIdAsync(string messageId);
        ValueTask<MeshMessage> RetrieveMessageByIdAsync(string messageId);
        ValueTask<MeshMessage> AcknowledgeMessageByIdAsync(string messageId);
    }
}
