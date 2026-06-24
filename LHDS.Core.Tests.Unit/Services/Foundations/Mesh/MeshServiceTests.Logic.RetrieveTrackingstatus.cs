// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Mesh;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldReturnRetrieveTrackingStatusAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;
            Message outputMessage = CreateRandomMessage();
            Message expectedMessage = outputMessage.DeepClone();

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(inputMessageId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(outputMessage);

            // when
            MeshMessage actualMeshMessage =
                await this.meshService.RetrieveTrackingStatusByIdAsync(
                    inputMessageId,
                    TestContext.Current.CancellationToken);

            // then
            actualMeshMessage.Should().BeEquivalentTo(expectedMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(inputMessageId, It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
