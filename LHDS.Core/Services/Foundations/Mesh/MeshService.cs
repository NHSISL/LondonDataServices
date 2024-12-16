// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public partial class MeshService : IMeshService
    {
        private readonly IMeshBroker meshBroker;
        private readonly ILoggingBroker loggingBroker;

        public MeshService(
            IMeshBroker meshBroker,
            ILoggingBroker loggingBroker)
        {
            this.meshBroker = meshBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<bool> ValidateMailboxAccessAsync() =>
            TryCatch(async () =>
            {
                return await this.meshBroker.HandshakeAsync();
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
            TryCatch(async () =>
            {
                ValidateMeshMessageOnSendMessage(mexTo, mexWorkflowId, fileContent);

                Message brokerSendMessage = await this.meshBroker
                    .SendMessageAsync(
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

                MeshMessage resultMeshMessage = MessageToMeshMessage(brokerSendMessage);

                return resultMeshMessage;
            });

        public ValueTask<MeshMessage> RetrieveTrackingStatusByIdAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateMessageId(messageId);
                Message brokerTrackMessage = await this.meshBroker.TrackMessageAsync(messageId);
                MeshMessage resultMeshMessage = MessageToMeshMessage(brokerTrackMessage);

                return resultMeshMessage;
            });

        public ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync() =>
            TryCatch(async () =>
            {
                List<string> messages = await this.meshBroker.RetrieveMessageIdsAsync();

                return messages;
            });

        public ValueTask<MeshMessage> RetrieveMessageByIdAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateMessageId(messageId);
                Message brokerRetrieveMessage = await this.meshBroker.RetrieveMessageAsync(messageId);
                MeshMessage resultMeshMessage = MessageToMeshMessage(brokerRetrieveMessage);

                return resultMeshMessage;
            });

        public ValueTask<bool> AcknowledgeMessageByIdAsync(string messageId) =>
            TryCatch(async () =>
            {
                ValidateMessageId(messageId);
                bool acknowledgedResult = await this.meshBroker.AcknowledgeMessageByIdAsync(messageId);

                return acknowledgedResult;
            });

        public static MeshMessage MessageToMeshMessage(Message message)
        {
            return new MeshMessage
            {
                MessageId = message.MessageId,
                Headers = message.Headers,
                FileContent = message.FileContent,
                TrackingInfo = ConvertToMeshMessageTrackingInfo(message.TrackingInfo)
            };
        }

        public static MessageTrackingInfo ConvertToMeshMessageTrackingInfo(TrackingInfo trackingInfo)
        {
            if (trackingInfo == null)
            {
                return null;
            }

            return new MessageTrackingInfo
            {
                AddressType = trackingInfo.AddressType,
                Checksum = trackingInfo.Checksum,
                ChunkCount = trackingInfo.ChunkCount,
                CompressFlag = trackingInfo.CompressFlag,
                DownloadTimestamp = trackingInfo.DownloadTimestamp,
                DtsId = trackingInfo.DtsId,
                EncryptedFlag = trackingInfo.EncryptedFlag,
                ExpiryTime = trackingInfo.ExpiryTime,
                FileName = trackingInfo.FileName,
                FileSize = trackingInfo.FileSize,
                IsCompressed = trackingInfo.IsCompressed,
                LocalId = trackingInfo.LocalId,
                MeshRecipientOdsCode = trackingInfo.MeshRecipientOdsCode,
                MessageId = trackingInfo.MessageId,
                MessageType = trackingInfo.MessageType,
                PartnerId = trackingInfo.PartnerId,
                Recipient = trackingInfo.Recipient,
                RecipientName = trackingInfo.RecipientName,
                RecipientOrgCode = trackingInfo.RecipientOrgCode,
                RecipientSmtp = trackingInfo.RecipientSmtp,
                Sender = trackingInfo.Sender,
                SenderName = trackingInfo.SenderName,
                SenderOdsCode = trackingInfo.SenderOdsCode,
                SenderOrgCode = trackingInfo.SenderOrgCode,
                SenderSmtp = trackingInfo.SenderSmtp,
                Status = trackingInfo.Status,
                StatusSuccess = trackingInfo.StatusSuccess,
                UploadTimestamp = trackingInfo.UploadTimestamp,
                Version = trackingInfo.Version,
                WorkflowId = trackingInfo.WorkflowId,
            };
        }

        public static TrackingInfo ConvertToMessageTrackingInfo(MessageTrackingInfo trackingInfo)
        {
            if (trackingInfo == null)
            {
                return null;
            }

            return new TrackingInfo
            {
                AddressType = trackingInfo.AddressType,
                Checksum = trackingInfo.Checksum,
                ChunkCount = trackingInfo.ChunkCount,
                CompressFlag = trackingInfo.CompressFlag,
                DownloadTimestamp = trackingInfo.DownloadTimestamp,
                DtsId = trackingInfo.DtsId,
                EncryptedFlag = trackingInfo.EncryptedFlag,
                ExpiryTime = trackingInfo.ExpiryTime,
                FileName = trackingInfo.FileName,
                FileSize = trackingInfo.FileSize,
                IsCompressed = trackingInfo.IsCompressed,
                LocalId = trackingInfo.LocalId,
                MeshRecipientOdsCode = trackingInfo.MeshRecipientOdsCode,
                MessageId = trackingInfo.MessageId,
                MessageType = trackingInfo.MessageType,
                PartnerId = trackingInfo.PartnerId,
                Recipient = trackingInfo.Recipient,
                RecipientName = trackingInfo.RecipientName,
                RecipientOrgCode = trackingInfo.RecipientOrgCode,
                RecipientSmtp = trackingInfo.RecipientSmtp,
                Sender = trackingInfo.Sender,
                SenderName = trackingInfo.SenderName,
                SenderOdsCode = trackingInfo.SenderOdsCode,
                SenderOrgCode = trackingInfo.SenderOrgCode,
                SenderSmtp = trackingInfo.SenderSmtp,
                Status = trackingInfo.Status,
                StatusSuccess = trackingInfo.StatusSuccess,
                UploadTimestamp = trackingInfo.UploadTimestamp,
                Version = trackingInfo.Version,
                WorkflowId = trackingInfo.WorkflowId,
            };
        }
    }
}
