// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public async Task ShouldPickupFileAndSendToMeshAsync()
        {
            //Given
            string messageId = GetRandomString();
            byte[] pdsFile = Encoding.ASCII.GetBytes(GetRandomString());
            string fileName = GetRandomString();
            string mexTo = this.pdsConfiguration.To;
            string mexWorkflowId = this.pdsConfiguration.WorkflowId;
            string mexSubject = string.Empty;
            string mexContentChecksum = string.Empty;
            string contentType = "text/plain";
            string contentEncoding = string.Empty;
            string accept = "application/json";
            Guid identifier = Guid.NewGuid();
            string mexLocalId = identifier.ToString();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(identifier);

            Message message = ComposeMessage.CreateFileMessage(
                mexTo: mexTo,
                mexWorkflowId: mexWorkflowId,
                fileContent: pdsFile,
                mexSubject: mexSubject,
                mexLocalId: identifier.ToString(),
                mexFileName: fileName,
                mexContentChecksum: mexContentChecksum,
                contentType: contentType,
                contentEncoding: contentEncoding,
                accept: accept);

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    pdsFile,
                    mexSubject,
                    mexLocalId,
                    fileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept))
                .ReturnsAsync(message);


            //When
            var actualPdsAudit = await pdsClient.PickupFileAndSendToMesh(pdsFile, fileName);

            //Then
            actualPdsAudit.Should().NotBeNull();
            actualPdsAudit.FileName.Should().Be(fileName);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Exactly(3));

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    pdsFile,
                    mexSubject,
                    mexLocalId,
                    fileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept),
                        Times.Once);

            await this.pdsAuditService.RemovePdsAuditByIdAsync(actualPdsAudit.Id);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
