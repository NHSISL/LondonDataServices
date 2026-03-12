// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAcknowledgeMessageByIdAsync()
        {
            // given
            MeshMessage randomMessage = CreateRandomMessage();
            MeshMessage storageMessage = randomMessage;
            bool isAcknowledged = true;

            this.meshServiceMock.Setup(service =>
                service.AcknowledgeMessageByIdAsync(randomMessage.MessageId))
                    .ReturnsAsync(isAcknowledged);

            // when
            await this.meshProcessingService.AcknowledgeMessageByIdAsync(randomMessage.MessageId);

            // then
            this.meshServiceMock.Verify(service =>
                service.AcknowledgeMessageByIdAsync(randomMessage.MessageId),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
