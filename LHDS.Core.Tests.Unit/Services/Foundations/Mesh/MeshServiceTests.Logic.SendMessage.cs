// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldReturnSendMessageAsyncWithNullTrackingInfo()
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

            dynamic dynamicMeshMessageProperties =
                CreateRandomDynamicMeshMessageProperties(
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

            Message randomMessage = new Message
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                FileContent = dynamicMeshMessageProperties.FileContent,
                TrackingInfo = MaptToMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            var outputMessage = randomMessage;

            MeshMessage randomMeshMessage = new MeshMessage
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                FileContent = dynamicMeshMessageProperties.FileContent,
                TrackingInfo = MaptToMeshMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            var expectedMeshMessage = randomMeshMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
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
                        .ReturnsAsync(outputMessage);

            // when
            MeshMessage actualMeshMessage =
                await this.meshService.SendMessageAsync(mexTo,
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
            actualMeshMessage.Should().BeEquivalentTo(expectedMeshMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
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

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

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

            dynamic dynamicMeshMessageProperties =
                CreateRandomDynamicMeshMessageProperties(
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

            var randomMessage = new Message
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                FileContent = dynamicMeshMessageProperties.FileContent,
                TrackingInfo = MaptToMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            var inputMessage = randomMessage;
            var outputMessage = inputMessage;

            MeshMessage randomMeshMessage = new MeshMessage
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                FileContent = dynamicMeshMessageProperties.FileContent,
                TrackingInfo = MaptToMeshMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            var inputMeshMessage = randomMeshMessage;
            var expectedMeshMessage = randomMeshMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
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
                    .ReturnsAsync(outputMessage);

            // when
            MeshMessage actualMeshMessage =
                await this.meshService.SendMessageAsync(
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
            actualMeshMessage.Should().BeEquivalentTo(expectedMeshMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
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

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
