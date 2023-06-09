// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
            message.Headers["Mex-WorkflowId"] = new List<string> { mexWorkflowId };
            List<Message> messages = new List<Message> { message };

            this.meshBrokerMock.Setup(broker => 
                broker.RetrieveMessageIdsAsync())
                    .ReturnsAsync(messageIds);

            foreach(var id in messageIds)
            {
                this.meshBrokerMock.Setup(broker =>
                    broker.RetrieveMessageAsync(id))
                        .ReturnsAsync(message);
            }

            //When
            var actualList = await pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();

            //Then
            actualList.Should().NotBeNull();
        }
    }
}
