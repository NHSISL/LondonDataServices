// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldReturnAknowledgeMessageByIdAsync()
        {
            // given
            string randomMailboxId = GetRandomMessage();
            string inputMailboxId = randomMailboxId;

            string randomMessageId = GetRandomMessage();
            string inputMessageId = randomMessageId;

            bool outputValidationResult = true;
            bool expectedValidationResult = outputValidationResult;

            this.meshBrokerMock.Setup(broker =>
                broker.AcknowledgeMessageByIdAsync(inputMailboxId, inputMessageId))
                    .ReturnsAsync(outputValidationResult);

            // when
            bool actualMeshValidation =
                await this.meshService.AcknowledgeMessageByIdAsync(inputMailboxId, inputMessageId);

            // then
            actualMeshValidation.Should().Be(expectedValidationResult);

            this.meshBrokerMock.Verify(broker =>
                broker.AcknowledgeMessageByIdAsync(inputMailboxId, inputMessageId),
                    Times.Once());

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
