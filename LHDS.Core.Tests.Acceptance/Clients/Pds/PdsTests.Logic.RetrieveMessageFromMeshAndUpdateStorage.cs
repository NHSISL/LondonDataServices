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

        [Fact]
        public async Task ShouldRetreiveCorrectNumberOfMessagesFromMeshAndUpdateStorageAsync()
        {
            //Given
            int randomNumber = GetRandomNumber();
            List<string> messageIds = GetRandomStrings(randomNumber);
            string mexWorkflowId = this.pdsConfiguration.WorkflowId;

            this.meshBrokerMock.Setup(broker =>
                broker.RetrieveMessageIdsAsync())
                    .ReturnsAsync(messageIds);

            List<Message> messages = new List<Message>();

            foreach (var id in messageIds)
            {
                Message message = CreateRandomMessage();
                message.MessageId = id;
                message.Headers["Mex-WorkflowID"] = new List<string> { mexWorkflowId };
                message.Headers["Mex-FileName"] = new List<string> { GetRandomString() };
                message.Headers["Mex-LocalID"] = new List<string> { Guid.NewGuid().ToString() };

                this.meshBrokerMock.Setup(broker =>
                    broker.RetrieveMessageAsync(id))
                        .ReturnsAsync(message);
            }

            //When
            var actualList = await pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();

            //Then
            actualList.Should().HaveCount(randomNumber);

            this.meshBrokerMock.Verify(broker =>
                broker.RetrieveMessageIdsAsync(),
                    Times.Once);

            foreach (var id in messageIds)
            {
                Message message = CreateRandomMessage();
                message.MessageId = id;
                message.Headers["Mex-WorkflowId"] = new List<string> { mexWorkflowId };
                message.Headers["Mex-FileName"] = new List<string> { GetRandomString() };
                message.Headers["Mex-LocalID"] = new List<string> { Guid.NewGuid().ToString() };

                this.meshBrokerMock.Verify(broker =>
                    broker.RetrieveMessageAsync(id),
                       Times.Once);
            }

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
