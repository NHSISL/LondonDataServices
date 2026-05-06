// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
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
        public async Task ShouldReturnRetrieveMessageByIdAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;
            Message outputMessage = CreateRandomMessage();

            this.meshBrokerMock.Setup(broker =>
                broker.RetrieveMessageAsync(inputMessageId, It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(outputMessage);

            MeshMessage expectedMeshMessage = new MeshMessage
            {
                MessageId = outputMessage.MessageId,
                Headers = outputMessage.Headers,
            };

            // when
            MeshMessage actualMeshMessage =
                await this.meshService.RetrieveMessageByIdAsync(
                    inputMessageId,
                    new MemoryStream(),
                    TestContext.Current.CancellationToken);

            // then
            actualMeshMessage.MessageId.Should().Be(expectedMeshMessage.MessageId);
            actualMeshMessage.Headers.Should().BeEquivalentTo(expectedMeshMessage.Headers);

            this.meshBrokerMock.Verify(broker =>
                broker.RetrieveMessageAsync(inputMessageId, It.IsAny<Stream>(), It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
