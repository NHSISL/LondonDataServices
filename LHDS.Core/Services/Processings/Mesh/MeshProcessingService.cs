// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Services.Foundations.Mesh;

namespace LHDS.Core.Services.Processings.Mesh
{
    public partial class MeshProcessingService : IMeshProcessingService
    {
        private readonly IMeshService meshService;
        private readonly ILoggingBroker loggingBroker;

        public MeshProcessingService(
            IMeshService meshService,
            ILoggingBroker loggingBroker)
        {
            this.meshService = meshService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<bool> ValidateMailboxAccessAsync(CancellationToken cancellationToken = default) =>
           TryCatch(async () =>
           {
               cancellationToken.ThrowIfCancellationRequested();

               return await this.meshService.ValidateMailboxAccessAsync(cancellationToken);
           });

        public ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync(
            CancellationToken cancellationToken = default) =>
         TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await this.meshService.RetrieveMessageIdsFromInboxAsync(cancellationToken);
            });

        public ValueTask<MeshMessage> RetrieveAndAcknowledgeMessageByIdAsync(
            string messageId,
            Stream outputStream,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateMeshArgs(messageId);

                MeshMessage retrievedMessage = await meshService
                    .RetrieveMessageByIdAsync(messageId, outputStream, cancellationToken);

                ValidateMeshMessageIsNotNull(retrievedMessage);
                await meshService.AcknowledgeMessageByIdAsync(messageId, cancellationToken);

                return retrievedMessage;
            });

        public ValueTask<MeshMessage> SendMessageAsync(
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
            CancellationToken cancellationToken = default) =>
            TryCatch((ReturningMessageMeshFunction)(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateMeshMessageOnSendMessage(mexTo, mexWorkflowId, content);

                MeshMessage sendMessageResult = await meshService.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    content,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    cancellationToken);

                ValidateSendMessage(sendMessageResult);

                MeshMessage trackMessage = await this.meshService
                    .RetrieveTrackingStatusByIdAsync(sendMessageResult.MessageId, cancellationToken);

                ValidateMeshMessageIsNotNull(trackMessage);
                sendMessageResult.TrackingInfo = trackMessage.TrackingInfo;

                return sendMessageResult;
            }));

        public ValueTask<MeshMessage> RetrieveMessageByIdAsync(
            string messageId,
            Stream outputStream,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateMeshArgs(messageId);

                MeshMessage retrievedMessage = await meshService
                    .RetrieveMessageByIdAsync(messageId, outputStream, cancellationToken);

                return retrievedMessage;
            });

        public ValueTask<bool> AcknowledgeMessageByIdAsync(
            string messageId,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateMeshArgs(messageId);

                return await meshService.AcknowledgeMessageByIdAsync(messageId, cancellationToken);
            });
    }
}
