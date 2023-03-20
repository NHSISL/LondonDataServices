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
        public async Task ShouldReturnRetrieveMessageIdsFromInboxAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            string inputMailboxId = randomMessageId;

            // when
            await this.meshProcessingService.RetrieveMessageIdsFromInboxAsync(inputMailboxId);

            // then
            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(inputMailboxId),
                    Times.Once());

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
