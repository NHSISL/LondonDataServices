// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using NEL.MESH.Clients.Mailboxes;
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
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
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

            Message message = ComposeMessage.CreateFileMessage(
                mexTo,
                mexWorkflowId,
                fileContent,
                mexLocalId: GetRandomString(),
                mexFileName: fileName,
                contentType: "text/plain");

            message.MessageId = messageId;
            List<Message> messages = new List<Message> { message };

            this.meshBrokerMock.SetupSequence(broker =>
                broker.RetrieveMessageIdsAsync())
                    .ReturnsAsync(messageIds)
                    .ReturnsAsync(new List<string>());

            foreach (var id in messageIds)
            {
                this.meshBrokerMock.Setup(broker =>
                    broker.RetrieveMessageAsync(id))
                        .ReturnsAsync(message);
            }

            List<Message> expectedMessages = messages.DeepClone();

            //When
            var actualMessageList = await this.optOutClient.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            //Then
            actualMessageList.Should().BeEquivalentTo(expectedMessages);

            this.meshBrokerMock.Verify(broker =>
                broker.RetrieveMessageIdsAsync(),
                    Times.Exactly(2));

            foreach (var id in messageIds)
            {
                this.meshBrokerMock.Verify(broker =>
                    broker.RetrieveMessageAsync(id),
                        Times.Once);

                this.meshBrokerMock.Verify(broker =>
                    broker.AcknowledgeMessageByIdAsync(id),
                        Times.Once);
            }

            foreach (OptOut optOut in randomOptOutList)
            {
                await this.optOutService.RemoveOptOutByIdAsync(optOut.Id);
            }

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
