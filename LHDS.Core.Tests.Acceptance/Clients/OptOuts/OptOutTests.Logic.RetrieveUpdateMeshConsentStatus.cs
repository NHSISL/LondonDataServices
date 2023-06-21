// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Clients;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using NEL.MESH.Clients.Mailboxes;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;
using Xunit.Sdk;
using static System.Net.Mime.MediaTypeNames;

namespace LHDS.Core.Tests.Acceptance.Clients.OptOuts
{

    public partial class OptOutTests
    {
        [Fact]
        public async Task ShouldRetrieveUpdateMeshConsentStatusAsync()
        {
            //Given
            string messageId = GetRandomString();
            List<string> messageIds = new List<string> { messageId };
            string mexWorkflowId = this.optOutConfiguration.WorkflowId;
            string mexLocalId = GetRandomString();
            string fileName = $"{optOutConfiguration.OutputFolder}/{mexLocalId}_deltaresponse.csv";
            byte[] fileContent = Encoding.ASCII.GetBytes(GetRandomString());
            string mexTo = this.optOutConfiguration.To;

            Message message = ComposeMessage.CreateFileMessage(
                mexTo,
                mexWorkflowId,
                fileContent,
                mexLocalId: GetRandomString(),
                mexFileName: fileName,
                contentType: "text/plain");

            message.MessageId = messageId;
            List<Message> messages = new List<Message> { message };

            this.meshBrokerMock.Setup(broker =>
                broker.RetrieveMessageIdsAsync())
                    .ReturnsAsync(messageIds);

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
            actualMessageList.Should().BeEquivalentTo(messages);

            this.meshBrokerMock.Verify(broker =>
                broker.RetrieveMessageIdsAsync(),
                    Times.Once);

            foreach (var id in messageIds)
            {
                this.meshBrokerMock.Verify(broker =>
                    broker.RetrieveMessageAsync(id),
                        Times.Once);

                this.blobStorageBrokerMock.Verify(broker =>
                    broker.InsertFileAsync(fileName, It.IsAny<Stream>()),
                        Times.Once());
            }

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
