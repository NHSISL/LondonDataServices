// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;

namespace LHDS.Core.Brokers.Mesh
{
    public interface IMeshBroker
    {
        ValueTask<bool> HandshakeAsync();

        ValueTask<Message> SendMessageAsync(
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

        ValueTask<Message> TrackMessageAsync(string messageId);
        ValueTask<List<string>> RetrieveMessageIdsAsync();
        ValueTask<Message> RetrieveMessageAsync(string messageId);
        ValueTask<bool> AcknowledgeMessageByIdAsync(string messageId);
    }
}
