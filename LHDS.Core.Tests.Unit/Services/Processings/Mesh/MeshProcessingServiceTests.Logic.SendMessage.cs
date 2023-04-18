// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
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
            MeshMessage randomMessage = CreateRandomMessage();
            MeshMessage storageSendMessage = randomMessage;
            MeshMessage storageMessage = randomMessage;

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(randomMessage))
                  .ReturnsAsync(storageSendMessage);

            this.meshServiceMock.Setup(service =>
                service.RetrieveTrackingStatusAsync(randomMessage.MessageId))
                    .ReturnsAsync(storageMessage);

            // when
            await this.meshProcessingService.SendMessageAsync(randomMessage);

            // then
            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(randomMessage),
                    Times.Once());

            this.meshServiceMock.Verify(service =>
                service.RetrieveTrackingStatusAsync(randomMessage.MessageId),
                    Times.Once());

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
