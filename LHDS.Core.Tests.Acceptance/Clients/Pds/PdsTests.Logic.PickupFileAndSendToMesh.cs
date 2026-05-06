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
            string mexSubject = string.Empty;
            string mexContentChecksum = string.Empty;
            string contentType = "text/plain";
            string contentEncoding = string.Empty;
            string accept = "application/json";

            Message message = new Message
            {
                MessageId = messageId,
                Headers = new Dictionary<string, List<string>>
                {
                    { "mex-to", new List<string> { mexTo } },
                    { "mex-workflowid", new List<string> { mexWorkflowId } },
                    { "mex-filename", new List<string> { fileName } }
                }
            };

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    It.IsAny<Stream>(),
                    mexSubject,
                    It.IsAny<string>(),
                    fileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(message);

            //When
            await using var pdsStream = new MemoryStream(pdsFile);
            var actualPdsAudit = await pdsClient.PickupFileAndSendToMesh(pdsStream, fileName);

            //Then
            actualPdsAudit.Should().NotBeNull();
            actualPdsAudit.FileName.Should().Be(fileName);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    It.IsAny<Stream>(),
                    mexSubject,
                    It.IsAny<string>(),
                    fileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            await this.pdsAuditService.RemovePdsAuditByIdAsync(actualPdsAudit.Id);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
