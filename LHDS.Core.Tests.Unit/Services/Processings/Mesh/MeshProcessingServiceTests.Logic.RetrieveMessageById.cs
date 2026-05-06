// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldReturnRetrieveMessageIdAsync()
        {
            // given
            MeshMessage randomMessage = CreateRandomMessage();
            MeshMessage storageMessage = randomMessage;

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageByIdAsync(
                    randomMessage.MessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
                    .ReturnsAsync(storageMessage);

            // when
            await this.meshProcessingService.RetrieveMessageByIdAsync(
                randomMessage.MessageId,
                Stream.Null);

            // then
            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageByIdAsync(
                    randomMessage.MessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
