// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;

namespace LHDS.Core.Services.Processings.Mesh
{
    public interface IMeshProcessingService
    {
        ValueTask<bool> ValidateMailboxAccessAsync();

        ValueTask<MeshMessage> SendMessageAsync(string mexTo,
            string mexWorkflowId,
            byte[] fileContent,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "application/octet-stream",
            string contentEncoding = "",
            string accept = "application/json");

        ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync();
        ValueTask<MeshMessage> RetrieveAndAcknowledgeMessageByIdAsync(string messageId);
        ValueTask<MeshMessage> RetrieveMessageByIdAsync(string messageId);
        ValueTask<bool> AcknowledgeMessageByIdAsync(string messageId);
    }
}
