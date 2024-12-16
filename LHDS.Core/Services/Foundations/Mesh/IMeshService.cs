// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public interface IMeshService
    {
        ValueTask<bool> ValidateMailboxAccessAsync();

        ValueTask<MeshMessage> SendMessageAsync(
            string mexTo,
            string mexWorkflowId,
            byte[] fileContent,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "application/octet-stream",
            string contentEncoding = "",
            string accept = "application/json");

        ValueTask<MeshMessage> RetrieveTrackingStatusByIdAsync(string messageId);
        ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync();
        ValueTask<MeshMessage> RetrieveMessageByIdAsync(string messageId);
        ValueTask<bool> AcknowledgeMessageByIdAsync(string inputMessageId);
    }
}
