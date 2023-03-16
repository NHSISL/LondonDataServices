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
        public async Task ShouldReturnSendMessageAsync()
        {
            // given
            string randomMessageId = GetRandomMessage();
            string inputMessageId = randomMessageId;
            string outputValidationResult = GetRandomMessage();
            string expectedValidationResult = outputValidationResult;

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(inputMessageId))
                    .ReturnsAsync(outputValidationResult);

            // when
            string actualMeshValidation =
                await this.meshService.SendMessageAsync(inputMessageId);

            // then
            actualMeshValidation.Should().Be(expectedValidationResult);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(inputMessageId),
                    Times.Once());

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
