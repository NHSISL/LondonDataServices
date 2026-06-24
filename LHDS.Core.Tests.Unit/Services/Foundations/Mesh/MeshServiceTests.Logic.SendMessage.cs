// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading;
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
                TrackingInfo = MaptToMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            var outputMessage = randomMessage;

            MeshMessage expectedMeshMessage = new MeshMessage
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                TrackingInfo = MaptToMeshMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            using Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    inputStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync(outputMessage);

            // when
            MeshMessage actualMeshMessage =
                await this.meshService.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    inputStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    TestContext.Current.CancellationToken);

            // then
            actualMeshMessage.Should().BeEquivalentTo(expectedMeshMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    inputStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    It.IsAny<CancellationToken>()),
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
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept);

            var outputMessage = new Message
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                TrackingInfo = MaptToMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            MeshMessage expectedMeshMessage = new MeshMessage
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                TrackingInfo = MaptToMeshMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            using Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    inputStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    It.IsAny<CancellationToken>()))
                    .ReturnsAsync(outputMessage);

            // when
            MeshMessage actualMeshMessage =
                await this.meshService.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    inputStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    TestContext.Current.CancellationToken);

            // then
            actualMeshMessage.Should().BeEquivalentTo(expectedMeshMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    inputStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
