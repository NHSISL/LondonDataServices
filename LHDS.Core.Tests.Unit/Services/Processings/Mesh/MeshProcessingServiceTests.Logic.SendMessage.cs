// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
            var randomMessageId = GetRandomString();
            MeshMessage randomSendMessage = CreateRandomMessage();
            MeshMessage inputMeshMessage = randomSendMessage;
            MeshMessage ouputMeshMessage = inputMeshMessage.DeepClone();
            inputMeshMessage.MessageId = null;
            MeshMessage randomTrackingMeshMessage = CreateRandomMessage();
            MeshMessage trackingOutputMeshMessage = ouputMeshMessage.DeepClone();
            trackingOutputMeshMessage.TrackingInfo = randomTrackingMeshMessage.TrackingInfo;
            MeshMessage expectedMessage = trackingOutputMeshMessage.DeepClone();

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(inputMeshMessage))
                  .ReturnsAsync(ouputMeshMessage);

            this.meshServiceMock.Setup(service =>
                service.RetrieveTrackingStatusByIdAsync(ouputMeshMessage.MessageId))
                    .ReturnsAsync(trackingOutputMeshMessage);

            // when
            MeshMessage actualMessage =
                await this.meshProcessingService.SendMessageAsync(inputMeshMessage);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(inputMeshMessage),
                    Times.Once());

            this.meshServiceMock.Verify(service =>
                service.RetrieveTrackingStatusByIdAsync(ouputMeshMessage.MessageId),
                    Times.Once());

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
