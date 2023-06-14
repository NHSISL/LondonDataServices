// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Clients.Mailboxes;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Pds
{
    public partial class PdsTests
    {
        [Fact]
        public async Task ShouldRetreiveMessagesFromMeshAndUpdateStorageAsync()
        {
            //Given
            string messageId = GetRandomString();
            List<string> messageIds = new List<string> { messageId };
            string mexWorkflowId = this.pdsConfiguration.WorkflowId;
            string fileName = GetRandomString();
            string mexLocalId = Guid.NewGuid().ToString();
            byte[] fileContent = Encoding.ASCII.GetBytes(GetRandomString());

            Message message = ComposeMessage.CreateFileMessage(
                mexWorkflowId, 
                fileName, 
                fileContent, 
                mexLocalId);

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

            //When
            var actualList = await pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();

            //Then
            actualList.Should().NotBeNull();
            actualList.Should().HaveCount(1);
            //actualList.All(a => messages.Contains(a.FileName)).Should().BeTrue();

            this.meshBrokerMock.Verify(broker =>
                broker.RetrieveMessageIdsAsync(),
                    Times.Once);

            foreach (var id in messageIds)
            {
                this.meshBrokerMock.Verify(broker =>
                    broker.RetrieveMessageAsync(id),
                        Times.Once);
            }

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
