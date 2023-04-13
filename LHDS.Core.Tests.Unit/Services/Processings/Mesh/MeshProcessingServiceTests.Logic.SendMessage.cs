// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldReturnSendMessageAsync()
        {
            // given
            Message randomMessage = CreateRandomMessage();

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
