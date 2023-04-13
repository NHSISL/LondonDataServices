// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FluentAssertions;
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
            dynamic meshMessageProperties =
                CreateRandomMeshMessageProperties();

            var randomMeshMessage = new Message
            {
                MessageId = meshMessageProperties.MessageId,
                Headers = meshMessageProperties.Headers,
                StringContent = meshMessageProperties.StringContent,
                FileContent = meshMessageProperties.FileContent,
                TrackingInfo = meshMessageProperties.TrackingInfo
            };

            Message inputMeshMessage = randomMeshMessage;
            Message expectedMeshMessage = inputMeshMessage;

            // when
            Message actualMeshMessage =
                await this.meshService.SendMessageAsync(inputMeshMessage);

            // then
            actualMeshMessage.Should().BeEquivalentTo(expectedMeshMessage);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(inputMeshMessage),
                    Times.Once());

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
