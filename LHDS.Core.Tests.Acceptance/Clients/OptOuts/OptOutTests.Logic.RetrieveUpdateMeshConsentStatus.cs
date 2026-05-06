// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.OptOuts
{

    public partial class OptOutTests
    {
        [Fact]
        public async Task ShouldRetrieveUpdateMeshConsentStatusAsync()
        {
            //Given
            DateTimeOffset currentDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            string timestamp = currentDateTimeOffset.ToString("yyyyMMddHHmmss");
            string messageId = GetRandomString();
            List<string> messageIds = new List<string> { messageId };
            string mexWorkflowId = this.optOutConfiguration.WorkflowId;
            string mexLocalId = GetRandomString();
            string fileName = $"{optOutConfiguration.OutputFolder}/{mexLocalId}_{timestamp}_Response.csv";
            string mexTo = this.optOutConfiguration.To;
            string batchReference = GetRandomString();
            bool withHeader = this.optOutConfiguration.OptOutFileHasHeader;
            bool withTrailingComma = this.optOutConfiguration.OptOutFileRequireTrailingComma;

            List<OptOut> randomOptOutList = CreateRandomOptOutsList(
                count: GetRandomNumber(),
                currentDateTimeOffset,
                batchReference);

            var csvOptOutList = new StringBuilder();

            foreach (OptOut optOut in randomOptOutList)
            {
                await this.optOutService.AddOptOutAsync(optOut);
            }

            randomOptOutList
                .ForEach(optOut =>
                    csvOptOutList.AppendLine($"{optOut.NhsNumber},"));

            byte[] fileContent = Encoding.ASCII.GetBytes(csvOptOutList.ToString());

            Message message = new Message
            {
                MessageId = messageId,
                Headers = new Dictionary<string, List<string>>
                {
                    { "mex-to", new List<string> { mexTo } },
                    { "mex-workflowid", new List<string> { mexWorkflowId } },
                    { "mex-localid", new List<string> { batchReference } },
                    { "mex-filename", new List<string> { fileName } }
                }
            };

            List<Message> messages = new List<Message> { message };

            this.meshBrokerMock.SetupSequence(broker =>
                broker.RetrieveMessageIdsAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(messageIds)
                    .ReturnsAsync(new List<string>());

            foreach (var id in messageIds)
            {
                this.meshBrokerMock.Setup(broker =>
                    broker.RetrieveMessageAsync(
                        id,
                        It.IsAny<Stream>(),
                        It.IsAny<CancellationToken>()))
                    .Callback<string, Stream, CancellationToken>((_, stream, _) =>
                        stream.Write(fileContent))
                    .ReturnsAsync(message);
            }

            string optOutContainer = dependencyBroker.Configuration
                .GetSection("blobStorage:BlobContainers:OptOut").Value;

            string deltaFileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_DeltaResponse.csv";

            this.blobStorageBrokerMock.Setup(broker =>
                broker.InsertFileAsync(It.IsAny<Stream>(), deltaFileName, optOutContainer))
                    .Returns(ValueTask.CompletedTask);

            List<Message> expectedMessages = new List<Message>(messages);

            //When
            var actualMessageList = await this.optOutClient.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            //Then
            actualMessageList.Should().BeEquivalentTo(expectedMessages);

            this.meshBrokerMock.Verify(broker =>
                broker.RetrieveMessageIdsAsync(It.IsAny<CancellationToken>()),
                    Times.Exactly(2));

            foreach (var id in messageIds)
            {
                this.meshBrokerMock.Verify(broker =>
                    broker.RetrieveMessageAsync(
                        id,
                        It.IsAny<Stream>(),
                        It.IsAny<CancellationToken>()),
                            Times.Once);

                this.meshBrokerMock.Verify(broker =>
                    broker.AcknowledgeMessageByIdAsync(id, It.IsAny<CancellationToken>()),
                        Times.Once);
            }

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(It.IsAny<Stream>(), deltaFileName, optOutContainer),
                    Times.Once);

            foreach (OptOut optOut in randomOptOutList)
            {
                await this.optOutService.RemoveOptOutByIdAsync(optOut.Id);
            }

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
