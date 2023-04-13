// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace LHDS.Core.Services.Processings.Mesh
{
    public interface IMeshProcessingService
    {
        ValueTask<bool> ValidateMailboxAccessAsync();
        ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync(string mailboxId);
        ValueTask<MeshMessage> RetrieveAndAcknowledgeMessageByIdAsync(MeshMessage message);
        ValueTask<MeshMessage> SendMessageAsync(MeshMessage message);
    }
}
