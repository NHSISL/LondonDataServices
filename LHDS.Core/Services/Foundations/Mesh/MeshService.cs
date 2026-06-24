// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
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

        public MeshService(IMeshBroker meshBroker, ILoggingBroker loggingBroker)
        {
            this.meshBroker = meshBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<bool> ValidateMailboxAccessAsync(CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await this.meshBroker.HandshakeAsync(cancellationToken);
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
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateMeshMessageOnSendMessage(mexTo, mexWorkflowId, content);

                Message brokerSendMessage = await this.meshBroker
                    .SendMessageAsync(
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

                return MessageToMeshMessage(brokerSendMessage);
            });

        public ValueTask<MeshMessage> RetrieveTrackingStatusByIdAsync(
            string messageId,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateMessageId(messageId);

                Message brokerTrackMessage =
                    await this.meshBroker.TrackMessageAsync(messageId, cancellationToken);

                return MessageToMeshMessage(brokerTrackMessage);
            });

        public ValueTask<List<string>> RetrieveMessageIdsFromInboxAsync(
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await this.meshBroker.RetrieveMessageIdsAsync(cancellationToken);
            });

        public ValueTask<MeshMessage> RetrieveMessageByIdAsync(
            string messageId,
            Stream outputStream,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateOnRetrieveMessageById(messageId, outputStream);

                Message brokerRetrieveMessage = await this.meshBroker
                    .RetrieveMessageAsync(messageId, outputStream, cancellationToken);

                return MessageToMeshMessage(brokerRetrieveMessage);
            });

        public ValueTask<bool> AcknowledgeMessageByIdAsync(
            string messageId,
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateMessageId(messageId);

                return await this.meshBroker.AcknowledgeMessageByIdAsync(messageId, cancellationToken);
            });

        private static MeshMessage MessageToMeshMessage(Message message)
        {
            return new MeshMessage
            {
                MessageId = message.MessageId,
                Headers = message.Headers,
                TrackingInfo = ConvertToMeshMessageTrackingInfo(message.TrackingInfo)
            };
        }

        private static MessageTrackingInfo ConvertToMeshMessageTrackingInfo(TrackingInfo trackingInfo)
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

        private static TrackingInfo ConvertToMessageTrackingInfo(MessageTrackingInfo trackingInfo)
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
