// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
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
            Message message = CreateRandomMessage();
            message.MessageId = messageId;
            message.Headers["Mex-WorkflowID"] = new List<string> { mexWorkflowId };
            message.Headers["Mex-FileName"] = new List<string> { GetRandomString() };
            message.Headers["Mex-LocalID"] = new List<string> { Guid.NewGuid().ToString() };
            List<Message> messages = new List<Message> { message };
            Guid identifier = Guid.NewGuid();

            this.meshBrokerMock.Setup(broker =>
                broker.RetrieveMessageIdsAsync())
                    .ReturnsAsync(messageIds);

            foreach (var id in messageIds)
            {
                this.meshBrokerMock.Setup(broker =>
                    broker.RetrieveMessageAsync(id))
                        .ReturnsAsync(message);
            }

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(identifier);

            //When
            var actualList = await pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();

            //Then
            actualList.Should().NotBeNull();
            actualList.Should().HaveCount(1);

            this.meshBrokerMock.Verify(broker =>
                broker.RetrieveMessageIdsAsync(),
                    Times.Once);

            foreach (var id in messageIds)
            {
                this.meshBrokerMock.Verify(broker =>
                    broker.RetrieveMessageAsync(id),
                        Times.Once);
            }

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
