// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Mesh;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldReturnSendMessageAsync()
        {
            // given
            string mexTo = GetRandomString();
            string mexWorkflowId = GetRandomString();
            byte[] fileContent = Encoding.UTF8.GetBytes(GetRandomString());
            string mexSubject = GetRandomString();
            string mexLocalId = GetRandomString();
            string mexFileName = GetRandomString();
            string mexContentChecksum = GetRandomString();
            string contentType = GetRandomString();
            string contentEncoding = GetRandomString();
            string accept = "text/plain";



            var randomMessageId = GetRandomString();

            MeshMessage randomMeshMessage = ComposeMessage.CreateMeshMessage(
                mexTo,
                mexWorkflowId,
                fileContent,
                mexSubject,
                mexLocalId,
                mexFileName,
                mexContentChecksum,
                contentType,
                contentEncoding,
                accept);

            randomMeshMessage.MessageId = randomMessageId;
            MeshMessage ouputMeshMessage = randomMeshMessage;
            MeshMessage randomTrackingMeshMessage = CreateRandomMessage();
            MeshMessage trackingOutputMeshMessage = ouputMeshMessage.DeepClone();
            trackingOutputMeshMessage.TrackingInfo = randomTrackingMeshMessage.TrackingInfo;
            MeshMessage expectedMessage = trackingOutputMeshMessage.DeepClone();

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept))
                        .ReturnsAsync(ouputMeshMessage);

            this.meshServiceMock.Setup(service =>
                service.RetrieveTrackingStatusByIdAsync(ouputMeshMessage.MessageId))
                    .ReturnsAsync(trackingOutputMeshMessage);

            // when
            MeshMessage actualMessage =
                await this.meshProcessingService.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept),
                        Times.Once);

            this.meshServiceMock.Verify(service =>
                service.RetrieveTrackingStatusByIdAsync(ouputMeshMessage.MessageId),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
