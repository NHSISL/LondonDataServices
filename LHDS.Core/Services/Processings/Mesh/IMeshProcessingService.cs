// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;

namespace LHDS.Core.Services.Processings.Mesh
{
    public interface IMeshProcessingService
    {
        ValueTask<bool> ValidateMailboxAccessAsync(CancellationToken cancellationToken = default);

        ValueTask<MeshMessage> SendMessageAsync(
            string mexTo,
            string mexWorkflowId,
            Stream content,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "application/octet-stream",
            string contentEncoding = "",
            string accept = "application/json",
            CancellationToken cancellationToken = default);

        ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync(CancellationToken cancellationToken = default);

        ValueTask<MeshMessage> RetrieveAndAcknowledgeMessageByIdAsync(
            string messageId,
            Stream outputStream,
            CancellationToken cancellationToken = default);

        ValueTask<MeshMessage> RetrieveMessageByIdAsync(
            string messageId,
            Stream outputStream,
            CancellationToken cancellationToken = default);

        ValueTask<bool> AcknowledgeMessageByIdAsync(
            string messageId,
            CancellationToken cancellationToken = default);
    }
}
