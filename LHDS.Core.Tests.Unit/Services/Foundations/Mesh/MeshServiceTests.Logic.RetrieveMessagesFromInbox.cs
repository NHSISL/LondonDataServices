// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldReturnRetrieveMessageIdsFromInboxAsync()
        {
            // given
            List<string> outputMessages = GetRandomMessages(GetRandomNumber());
            List<string> expectedMessages = outputMessages;

            this.meshBrokerMock.Setup(broker =>
                broker.RetrieveMessageIdsAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(outputMessages);

            // when
            List<string> actualMessages =
                await this.meshService.RetrieveMessageIdsFromInboxAsync(
                    TestContext.Current.CancellationToken);

            // then
            actualMessages.Should().BeSameAs(expectedMessages);

            this.meshBrokerMock.Verify(broker =>
                broker.RetrieveMessageIdsAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
