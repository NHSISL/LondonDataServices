// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldReturnRetrieveMessageIdAndAcknowledgeAsync()
        {
            // given
            MeshMessage randomMessage = CreateRandomMessage();
            MeshMessage storageMessage = randomMessage;

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageByIdAsync(randomMessage.MessageId, It.IsAny<Stream>(), default))
                    .ReturnsAsync(storageMessage);

            // when
            await this.meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(
                randomMessage.MessageId, new MemoryStream());

            // then
            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageByIdAsync(randomMessage.MessageId, It.IsAny<Stream>(), default),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
               service.AcknowledgeMessageByIdAsync(randomMessage.MessageId),
                   Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
