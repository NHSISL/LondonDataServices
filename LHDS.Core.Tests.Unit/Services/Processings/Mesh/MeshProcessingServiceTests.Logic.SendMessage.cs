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
            MeshMessage randomSendMessage = CreateRandomSendMessage();
            MeshMessage storageSendMessage = randomSendMessage.DeepClone();
            MeshMessage storageMessage = storageSendMessage.DeepClone();
            storageMessage.MessageId = randomMessageId;
            MeshMessage randomTrackingMessage = CreateRandomMessage();
            MeshMessage trackingStorageMessage = randomTrackingMessage;
            MeshMessage expectedMessage = storageMessage.DeepClone();
            expectedMessage.TrackingInfo = trackingStorageMessage.TrackingInfo;

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(storageSendMessage))
                  .ReturnsAsync(storageMessage);

            this.meshServiceMock.Setup(service =>
                service.RetrieveTrackingStatusByIdAsync(storageMessage.MessageId))
                    .ReturnsAsync(trackingStorageMessage);

            // when
            MeshMessage actualMessage =
                await this.meshProcessingService.SendMessageAsync(storageSendMessage);

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(storageSendMessage),
                    Times.Once());

            this.meshServiceMock.Verify(service =>
                service.RetrieveTrackingStatusByIdAsync(storageMessage.MessageId),
                    Times.Once());

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
