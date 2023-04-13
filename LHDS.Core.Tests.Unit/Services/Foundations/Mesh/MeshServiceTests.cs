// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
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

        public MeshServiceTests()
        {
            this.meshBrokerMock = new Mock<IMeshBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.meshService = new MeshService(
                meshBroker: this.meshBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }
        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNumber() =>
          new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

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

        public static Dictionary<string, List<string>> GetRandomDictionary(int entryCount)
        {
            var dictionary = new Dictionary<string, List<string>>();

            for (int i = 0; i < entryCount; i++)
            {
                string key = GetRandomString();
                List<string> values = RandomStringList(GetRandomNumber());
                dictionary[key] = values;
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

        public static dynamic CreateRandomMeshMessageProperties()
        {
            return new
            {
                MessageId = GetRandomString(),
                Headers = GetRandomDictionary(GetRandomNumber()),
                StringContent = GetRandomString(),
                FileContent = Encoding.ASCII.GetBytes(GetRandomString()),
                TrackingInfo = GetRandomTrackingInfo()
            };
        }

        private static string GetRandomMessage() =>
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

        private string GetValidationSummary(IDictionary data)
        {
            StringBuilder validationSummary = new StringBuilder();

            foreach (DictionaryEntry entry in data)
            {
                string errorSummary = ((List<string>)entry.Value)
                    .Select((string value) => value)
                    .Aggregate((string current, string next) => current + ", " + next);

                validationSummary.Append($"{entry.Key} => {errorSummary};  ");
            }

            return validationSummary.ToString();
        }
    }
}
