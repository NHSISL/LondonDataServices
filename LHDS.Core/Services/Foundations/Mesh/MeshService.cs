// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
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

        public async ValueTask<MeshMessage> SendMessageAsync(MeshMessage message)
        {
            Message convertedMessage = MeshMessageToMessage(message);
            Message brokerSendMessage =  await this.meshBroker.SendMessageAsync(convertedMessage);
            MeshMessage resultMeshMessage = MessageToMeshMessage(brokerSendMessage);

            return resultMeshMessage;
        }

        public ValueTask<MeshMessage> SendFileAsync(MeshMessage message) =>
            throw new System.NotImplementedException();

        public ValueTask<MeshMessage> RetrieveTrackingStatusAsync(string messageId) =>
            throw new NotImplementedException();

        public ValueTask<List<string>> RetrieveMessagesFromInboxAsync() =>
            throw new System.NotImplementedException();

        public ValueTask<MeshMessage> RetrieveMessageByIdAsync(string messageId) =>
            throw new System.NotImplementedException();

        public ValueTask<bool> AcknowledgeMessageByIdAsync(string messageId) =>
            throw new System.NotImplementedException();

        private static Message MeshMessageToMessage(MeshMessage meshMessage)
        {
            return new Message
            {
                MessageId = meshMessage.MessageId,
                Headers = meshMessage.Headers,
                StringContent = meshMessage.StringContent,
                FileContent = meshMessage.FileContent,
                TrackingInfo = ConvertToMessage(meshMessage.TrackingInfo)
            };
        }

        private static MeshMessage MessageToMeshMessage(Message message)
        {
            return new MeshMessage
            {
                MessageId = message.MessageId,
                Headers = message.Headers,
                StringContent = message.StringContent,
                FileContent = message.FileContent,
                TrackingInfo = ConvertToMeshMessage(message.TrackingInfo)
            };
        }

        private static LHDS.Core.Models.Foundations.Mesh.TrackingInfo ConvertToMeshMessage(NEL.MESH.Models.Foundations.Mesh.TrackingInfo nelTrackingInfo)
        {
            return new LHDS.Core.Models.Foundations.Mesh.TrackingInfo
            {
                AddressType = nelTrackingInfo.AddressType,
                Checksum = nelTrackingInfo.Checksum,
                ChunkCount = nelTrackingInfo.ChunkCount,
                CompressFlag = nelTrackingInfo.CompressFlag,
                DownloadTimestamp = nelTrackingInfo.DownloadTimestamp,
                DtsId = nelTrackingInfo.DtsId,
                EncryptedFlag = nelTrackingInfo.EncryptedFlag,
                ExpiryTime = nelTrackingInfo.ExpiryTime,
                FileName = nelTrackingInfo.FileName,
                FileSize = nelTrackingInfo.FileSize,
                IsCompressed = nelTrackingInfo.IsCompressed,
                LocalId = nelTrackingInfo.LocalId,
                MeshRecipientOdsCode = nelTrackingInfo.MeshRecipientOdsCode,
                MessageId = nelTrackingInfo.MessageId,
                MessageType = nelTrackingInfo.MessageType,
                PartnerId = nelTrackingInfo.PartnerId,
                Recipient = nelTrackingInfo.Recipient,
                RecipientName = nelTrackingInfo.RecipientName,
                RecipientOrgCode = nelTrackingInfo.RecipientOrgCode,
                RecipientSmtp = nelTrackingInfo.RecipientSmtp,
                Sender = nelTrackingInfo.Sender,
                SenderName = nelTrackingInfo.SenderName,
                SenderOdsCode = nelTrackingInfo.SenderOdsCode,
                SenderOrgCode = nelTrackingInfo.SenderOrgCode,
                SenderSmtp = nelTrackingInfo.SenderSmtp,
                Status = nelTrackingInfo.Status,
                StatusSuccess = nelTrackingInfo.StatusSuccess,
                UploadTimestamp = nelTrackingInfo.UploadTimestamp,
                Version = nelTrackingInfo.Version,
                WorkflowId = nelTrackingInfo.WorkflowId,
            };
        }

        public static NEL.MESH.Models.Foundations.Mesh.TrackingInfo ConvertToMessage(LHDS.Core.Models.Foundations.Mesh.TrackingInfo lhdsTrackingInfo)
        {
            return new NEL.MESH.Models.Foundations.Mesh.TrackingInfo
            {
                AddressType = lhdsTrackingInfo.AddressType,
                Checksum = lhdsTrackingInfo.Checksum,
                ChunkCount = lhdsTrackingInfo.ChunkCount,
                CompressFlag = lhdsTrackingInfo.CompressFlag,
                DownloadTimestamp = lhdsTrackingInfo.DownloadTimestamp,
                DtsId = lhdsTrackingInfo.DtsId,
                EncryptedFlag = lhdsTrackingInfo.EncryptedFlag,
                ExpiryTime = lhdsTrackingInfo.ExpiryTime,
                FileName = lhdsTrackingInfo.FileName,
                FileSize = lhdsTrackingInfo.FileSize,
                IsCompressed = lhdsTrackingInfo.IsCompressed,
                LocalId = lhdsTrackingInfo.LocalId,
                MeshRecipientOdsCode = lhdsTrackingInfo.MeshRecipientOdsCode,
                MessageId = lhdsTrackingInfo.MessageId,
                MessageType = lhdsTrackingInfo.MessageType,
                PartnerId = lhdsTrackingInfo.PartnerId,
                Recipient = lhdsTrackingInfo.Recipient,
                RecipientName = lhdsTrackingInfo.RecipientName,
                RecipientOrgCode = lhdsTrackingInfo.RecipientOrgCode,
                RecipientSmtp = lhdsTrackingInfo.RecipientSmtp,
                Sender = lhdsTrackingInfo.Sender,
                SenderName = lhdsTrackingInfo.SenderName,
                SenderOdsCode = lhdsTrackingInfo.SenderOdsCode,
                SenderOrgCode = lhdsTrackingInfo.SenderOrgCode,
                SenderSmtp = lhdsTrackingInfo.SenderSmtp,
                Status = lhdsTrackingInfo.Status,
                StatusSuccess = lhdsTrackingInfo.StatusSuccess,
                UploadTimestamp = lhdsTrackingInfo.UploadTimestamp,
                Version = lhdsTrackingInfo.Version,
                WorkflowId = lhdsTrackingInfo.WorkflowId,
            };
        }
    }
}
