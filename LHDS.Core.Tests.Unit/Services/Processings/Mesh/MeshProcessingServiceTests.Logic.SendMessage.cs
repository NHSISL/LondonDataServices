// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading;
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

            using Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(
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
                        .ReturnsAsync(ouputMeshMessage);

            this.meshServiceMock.Setup(service =>
                service.RetrieveTrackingStatusByIdAsync(
                    ouputMeshMessage.MessageId,
                    It.IsAny<CancellationToken>()))
                    .ReturnsAsync(trackingOutputMeshMessage);

            // when
            MeshMessage actualMessage =
                await this.meshProcessingService.SendMessageAsync(
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
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(
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

            this.meshServiceMock.Verify(service =>
                service.RetrieveTrackingStatusByIdAsync(
                    ouputMeshMessage.MessageId,
                    It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
