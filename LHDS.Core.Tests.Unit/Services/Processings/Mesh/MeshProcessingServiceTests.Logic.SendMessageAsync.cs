// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
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
            string randomMailboxId = GetRandomString();
            string inputMailboxId = randomMailboxId;
            string randomMessageId = GetRandomString();
            string inputMessageId = randomMessageId;

            // when
            await this.meshProcessingService.SendMessageAsync(inputMailboxId, inputMessageId);

            // then
            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(inputMessageId),
                    Times.Once());

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
