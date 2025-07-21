// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
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
            string pdsFileContainer = "pds";
            string messageId = GetRandomString();
            List<string> messageIds = new List<string> { messageId };
            string mexWorkflowId = this.pdsConfiguration.WorkflowId;
            string fileName = "RESP_MPTREQ_CCYYMMDDHHMISS_CCYYMMDDHHMISS.csv";
            string mexLocalId = Guid.NewGuid().ToString();
            byte[] fileContent = Encoding.ASCII.GetBytes(GetRandomString());

            Message message = ComposeMessage.CreateFileMessage(
                mexTo: this.pdsConfiguration.To,
                mexWorkflowId,
                fileContent,
                mexSubject: GetRandomString(),
                mexLocalId,
                mexFileName: fileName);

            string fileNameReturn =
                $"{this.pdsConfiguration.OutputFolder}/MPTREQ_CCYYMMDDHHMISS_CCYYMMDDHHMISS.csv";

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

            //When
            List<PdsAudit> actualList = await pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();

            //Then
            actualList.Should().HaveCount(1);

            this.meshBrokerMock.Verify(broker =>
                broker.RetrieveMessageIdsAsync(),
                    Times.Exactly(2));

            foreach (var item in actualList)
            {
                this.meshBrokerMock.Verify(broker =>
                    broker.RetrieveMessageAsync(item.MessageId),
                        Times.Once);

                this.blobStorageBrokerMock.Verify(broker =>
                    broker.InsertFileAsync(It.IsAny<Stream>(), fileNameReturn, pdsFileContainer),
                        Times.Once());

                this.meshBrokerMock.Verify(broker =>
                    broker.AcknowledgeMessageByIdAsync(item.MessageId),
                        Times.Once);

                await this.pdsAuditService.RemovePdsAuditByIdAsync(item.Id);
            }

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
