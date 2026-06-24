// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Services.Foundations.Mesh;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        private readonly Mock<IMeshBroker> meshBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IMeshService meshService;
        private readonly ICompareLogic compareLogic;

        public MeshServiceTests()
        {
            this.meshBrokerMock = new Mock<IMeshBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.meshService = new MeshService(
                meshBroker: this.meshBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNumber() =>
          new IntRange(min: 2, max: 10).GetValue();

        private static dynamic GetRandomTrackingInfo()
        {
            return new
            {
                AddressType = GetRandomString(),
                Checksum = GetRandomString(),
                ChunkCount = GetRandomNumber(),
                CompressFlag = GetRandomString(),
                DownloadTimestamp = GetRandomString(),
                DtsId = GetRandomString(),
                EncryptedFlag = GetRandomString(),
                ExpiryTime = GetRandomString(),
                FileName = GetRandomString(),
                FileSize = GetRandomNumber(),
                IsCompressed = GetRandomString(),
                LocalId = GetRandomString(),
                MeshRecipientOdsCode = GetRandomString(),
                MessageId = GetRandomString(),
                MessageType = GetRandomString(),
                PartnerId = GetRandomString(),
                Recipient = GetRandomString(),
                RecipientName = GetRandomString(),
                RecipientOrgCode = GetRandomString(),
                RecipientSmtp = GetRandomString(),
                Sender = GetRandomString(),
                SenderName = GetRandomString(),
                SenderOdsCode = GetRandomString(),
                SenderOrgCode = GetRandomString(),
                SenderSmtp = GetRandomString(),
                Status = GetRandomString(),
                StatusSuccess = GetRandomString(),
                UploadTimestamp = GetRandomString(),
                Version = GetRandomString(),
                WorkflowId = GetRandomString()
            };
        }

        private static MessageTrackingInfo MaptToMeshMessageTrackingInfo(dynamic dynamicTrackingInfo)
        {
            return new MessageTrackingInfo
            {
                AddressType = dynamicTrackingInfo.AddressType,
                Checksum = dynamicTrackingInfo.Checksum,
                ChunkCount = dynamicTrackingInfo.ChunkCount,
                CompressFlag = dynamicTrackingInfo.CompressFlag,
                DownloadTimestamp = dynamicTrackingInfo.DownloadTimestamp,
                DtsId = dynamicTrackingInfo.DtsId,
                EncryptedFlag = dynamicTrackingInfo.EncryptedFlag,
                ExpiryTime = dynamicTrackingInfo.ExpiryTime,
                FileName = dynamicTrackingInfo.FileName,
                FileSize = dynamicTrackingInfo.FileSize,
                IsCompressed = dynamicTrackingInfo.IsCompressed,
                LocalId = dynamicTrackingInfo.LocalId,
                MeshRecipientOdsCode = dynamicTrackingInfo.MeshRecipientOdsCode,
                MessageId = dynamicTrackingInfo.MessageId,
                MessageType = dynamicTrackingInfo.MessageType,
                PartnerId = dynamicTrackingInfo.PartnerId,
                Recipient = dynamicTrackingInfo.Recipient,
                RecipientName = dynamicTrackingInfo.RecipientName,
                RecipientOrgCode = dynamicTrackingInfo.RecipientOrgCode,
                RecipientSmtp = dynamicTrackingInfo.RecipientSmtp,
                Sender = dynamicTrackingInfo.Sender,
                SenderName = dynamicTrackingInfo.SenderName,
                SenderOdsCode = dynamicTrackingInfo.SenderOdsCode,
                SenderOrgCode = dynamicTrackingInfo.SenderOrgCode,
                SenderSmtp = dynamicTrackingInfo.SenderSmtp,
                Status = dynamicTrackingInfo.Status,
                StatusSuccess = dynamicTrackingInfo.StatusSuccess,
                UploadTimestamp = dynamicTrackingInfo.UploadTimestamp,
                Version = dynamicTrackingInfo.Version,
                WorkflowId = dynamicTrackingInfo.WorkflowId,
            };
        }

        private static TrackingInfo MaptToMessageTrackingInfo(dynamic dynamicTrackingInfo)
        {
            return new TrackingInfo
            {
                AddressType = dynamicTrackingInfo.AddressType,
                Checksum = dynamicTrackingInfo.Checksum,
                ChunkCount = dynamicTrackingInfo.ChunkCount,
                CompressFlag = dynamicTrackingInfo.CompressFlag,
                DownloadTimestamp = dynamicTrackingInfo.DownloadTimestamp,
                DtsId = dynamicTrackingInfo.DtsId,
                EncryptedFlag = dynamicTrackingInfo.EncryptedFlag,
                ExpiryTime = dynamicTrackingInfo.ExpiryTime,
                FileName = dynamicTrackingInfo.FileName,
                FileSize = dynamicTrackingInfo.FileSize,
                IsCompressed = dynamicTrackingInfo.IsCompressed,
                LocalId = dynamicTrackingInfo.LocalId,
                MeshRecipientOdsCode = dynamicTrackingInfo.MeshRecipientOdsCode,
                MessageId = dynamicTrackingInfo.MessageId,
                MessageType = dynamicTrackingInfo.MessageType,
                PartnerId = dynamicTrackingInfo.PartnerId,
                Recipient = dynamicTrackingInfo.Recipient,
                RecipientName = dynamicTrackingInfo.RecipientName,
                RecipientOrgCode = dynamicTrackingInfo.RecipientOrgCode,
                RecipientSmtp = dynamicTrackingInfo.RecipientSmtp,
                Sender = dynamicTrackingInfo.Sender,
                SenderName = dynamicTrackingInfo.SenderName,
                SenderOdsCode = dynamicTrackingInfo.SenderOdsCode,
                SenderOrgCode = dynamicTrackingInfo.SenderOrgCode,
                SenderSmtp = dynamicTrackingInfo.SenderSmtp,
                Status = dynamicTrackingInfo.Status,
                StatusSuccess = dynamicTrackingInfo.StatusSuccess,
                UploadTimestamp = dynamicTrackingInfo.UploadTimestamp,
                Version = dynamicTrackingInfo.Version,
                WorkflowId = dynamicTrackingInfo.WorkflowId,
            };
        }

        public static Dictionary<string, List<string>> CreateHeaders(
            string mexTo,
            string mexWorkflowId,
            string mexSubject = null,
            string mexLocalId = null,
            string mexFileName = null,
            string mexContentChecksum = null,
            string contentType = null,
            string contentEncoding = null,
            string accept = null)
        {
            var dictionary = new Dictionary<string, List<string>>();

            if (!string.IsNullOrEmpty(mexTo))
            {
                dictionary.Add("mex-to", new List<string> { mexTo });
            }

            if (!string.IsNullOrEmpty(mexWorkflowId))
            {
                dictionary.Add("mex-workflowid", new List<string> { mexWorkflowId });
            }

            if (!string.IsNullOrEmpty(mexSubject))
            {
                dictionary.Add("mex-subject", new List<string> { mexSubject });
            }

            if (!string.IsNullOrEmpty(mexLocalId))
            {
                dictionary.Add("mex-localid", new List<string> { mexLocalId });
            }

            if (!string.IsNullOrEmpty(mexFileName))
            {
                dictionary.Add("mex-filename", new List<string> { mexFileName });
            }

            if (!string.IsNullOrEmpty(mexContentChecksum))
            {
                dictionary.Add("mex-content-checksum", new List<string> { mexContentChecksum });
            }

            if (!string.IsNullOrEmpty(contentType))
            {
                dictionary.Add("content-type", new List<string> { contentType });
            }

            if (!string.IsNullOrEmpty(contentEncoding))
            {
                dictionary.Add("content-encoding", new List<string> { contentEncoding });
            }

            if (!string.IsNullOrEmpty(accept))
            {
                dictionary.Add("accept", new List<string> { accept });
            }

            return dictionary;
        }

        private static List<string> RandomStringList(int count)
        {
            var list = new List<string>();

            for (int i = 0; i < count; i++)
            {
                list.Add(GetRandomString());
            }

            return list;
        }

        public static dynamic CreateRandomDynamicMeshMessageProperties(
            string mexTo,
            string mexWorkflowId,
            string mexSubject,
            string mexLocalId,
            string mexFileName,
            string mexContentChecksum,
            string contentType,
            string contentEncoding,
            string accept)
        {
            return new
            {
                MessageId = GetRandomString(),

                Headers = CreateHeaders(
                    mexTo,
                    mexWorkflowId,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept),

                StringContent = GetRandomString(),
                TrackingInfo = GetRandomTrackingInfo()
            };
        }

        private Expression<Func<Message, bool>> SameMessageAs(Message expectedMessage)
        {
            return actualMessage =>
                this.compareLogic.Compare(expectedMessage, actualMessage)
                    .AreEqual;
        }

        private static string GetRandomString() =>
          new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static List<string> GetRandomMessages(int count)
        {
            var messages = new List<string>();
            for (int i = 0; i < count; i++)
            {
                string message = new MnemonicString(wordCount: GetRandomNumber()).GetValue();
                messages.Add(message);
            }
            return messages;
        }

        private static MeshMessage CreateRandomMeshMessage() =>
            CreateMeshMessageFiller().Create();

        private static Filler<MeshMessage> CreateMeshMessageFiller()
        {
            var filler = new Filler<MeshMessage>();
            filler.Setup().OnProperty(message => message.Headers)
                .Use(new Dictionary<string, List<string>>());

            return filler;
        }

        private static Message CreateRandomMessage() =>
            CreateMessageFiller().Create();

        private static Filler<Message> CreateMessageFiller()
        {
            var filler = new Filler<Message>();
            filler.Setup().OnProperty(message => message.Headers)
                .Use(new Dictionary<string, List<string>>());

            return filler;
        }
    }
}
