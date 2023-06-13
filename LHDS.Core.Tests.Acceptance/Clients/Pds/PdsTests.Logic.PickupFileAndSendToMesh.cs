// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
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
        public async Task ShouldPickupFileAndSendToMeshAsync()
        {

            //Given
            string messageId = GetRandomString();
            byte[] pdsFile = Encoding.ASCII.GetBytes(GetRandomString());    
            string fileName = GetRandomString();
            string mexTo = this.pdsConfiguration.To;
            string mexWorkflowId = this.pdsConfiguration.WorkflowId;
            Guid localId = Guid.NewGuid();
            Message message = CreateRandomMessage();
            message.MessageId = messageId;
            message.Headers["Mex-To"] = new List<string> { mexTo };
            message.Headers["Mex-WorkflowID"] = new List<string> { mexWorkflowId };
            message.Headers["Mex-FileName"] = new List<string> { fileName };
            message.Headers["Mex-Subject"] = new List<string> { GetRandomString() };
            message.Headers["Mex-Content-Checksum"] = new List<string> { GetRandomString() };
            message.Headers["Mex-Encoding"] = new List<string> { GetRandomString() };
            message.Headers["Mex-Content-Type"] = new List<string> { "application/octet-stream" };
            message.Headers["Mex-Accept"] = new List<string> { "application/json" };

            this.identifierBrokerMock.Setup(broker => 
                broker.GetIdentifier())
                    .Returns(localId);

            message.Headers["Mex-LocalID"] = new List<string> { localId.ToString() };
            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    pdsFile,
                    string.Empty,
                    localId.ToString(),
                    fileName,
                    string.Empty,
                    "text/plain",
                    string.Empty,
                    "application/json"))
                        .ReturnsAsync(message);

            //When
            var actualPdsAudit = await pdsClient.PickupFileAndSendToMesh(pdsFile, fileName);

            //Then
            actualPdsAudit.Should().NotBeNull();
            actualPdsAudit.FileName.Should().Be(fileName);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    pdsFile,
                    string.Empty,
                    localId.ToString(),
                    fileName,
                    string.Empty,
                    "text/plain",
                    string.Empty,
                    "application/json"),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
