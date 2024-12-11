// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
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

        public ValueTask<bool> ValidateMailboxAccessAsync() =>
           TryCatch(async () =>
           {
               return await this.meshService.ValidateMailboxAccessAsync();
           });

        public ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync() =>
         TryCatch(async () =>
            {
                return await this.meshService.RetrieveMessageIdsFromInboxAsync();
            });

        public ValueTask<MeshMessage> RetrieveAndAcknowledgeMessageByIdAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateMeshArgs(messageId);
                MeshMessage retrievedMessage = await meshService.RetrieveMessageByIdAsync(messageId);
                ValidateMeshMessageIsNotNull(retrievedMessage);
                bool ackResult = await meshService.AcknowledgeMessageByIdAsync(messageId);

                return retrievedMessage;
            });

        public ValueTask<MeshMessage> SendMessageAsync(
            string mexTo,
            string mexWorkflowId,
            byte[] fileContent,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "application/octet-stream",
            string contentEncoding = "",
            string accept = "application/json") =>
            TryCatch((ReturningMessageMeshFunction)(async () =>
            {
                ValidateMeshMessageOnSendMessage(mexTo, mexWorkflowId, fileContent);

                MeshMessage sendMessageResult = await meshService.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept);

                ValidateSendMessage(sendMessageResult);

                MeshMessage trackMessage =
                    await this.meshService.RetrieveTrackingStatusByIdAsync(sendMessageResult.MessageId);

                ValidateMeshMessageIsNotNull(trackMessage);
                sendMessageResult.TrackingInfo = trackMessage.TrackingInfo;

                return sendMessageResult;
            }));

        public ValueTask<MeshMessage> RetrieveMessageByIdAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateMeshArgs(messageId);
                MeshMessage retrievedMessage = await meshService.RetrieveMessageByIdAsync(messageId);

                return retrievedMessage;
            });

        public ValueTask<bool> AcknowledgeMessageByIdAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateMeshArgs(messageId);
                bool ackResult = await meshService.AcknowledgeMessageByIdAsync(messageId);

                return ackResult;
            });
    }
}
