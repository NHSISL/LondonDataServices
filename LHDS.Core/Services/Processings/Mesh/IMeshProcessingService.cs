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
        ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync();
        ValueTask<MeshMessage> RetrieveAndAcknowledgeMessageByIdAsync(string messageId);
        ValueTask<MeshMessage> SendMessageAsync(MeshMessage message);
    }
}
