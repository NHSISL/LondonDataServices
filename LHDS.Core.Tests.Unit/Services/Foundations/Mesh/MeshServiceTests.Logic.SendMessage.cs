// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
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
        public async Task ShouldReturnSendMessageAsync()
        {
            // given
            dynamic dynamicMeshMessageProperties =
                CreateRandomMeshMessageProperties();

            var randomMessage = new Message
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                StringContent = dynamicMeshMessageProperties.StringContent,
                FileContent = dynamicMeshMessageProperties.FileContent,
                TrackingInfo = MaptToMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            var inputMessage = randomMessage;
            var outputMessage = inputMessage;

            MeshMessage randomMeshMessage = new MeshMessage
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                StringContent = dynamicMeshMessageProperties.StringContent,
                FileContent = dynamicMeshMessageProperties.FileContent,
                TrackingInfo = MaptToMeshMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            }; 

            var inputMeshMessage = randomMeshMessage;
            var expectedMeshMessage = randomMeshMessage;

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(It.Is(SameMessageAs(inputMessage))))
                    .ReturnsAsync(outputMessage);

            // when
            MeshMessage actualMeshMessage =
                await this.meshService.SendMessageAsync(inputMeshMessage);

            // then
            actualMeshMessage.Should().BeEquivalentTo(expectedMeshMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(It.Is(SameMessageAs(inputMessage))),
                    Times.Once());

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
