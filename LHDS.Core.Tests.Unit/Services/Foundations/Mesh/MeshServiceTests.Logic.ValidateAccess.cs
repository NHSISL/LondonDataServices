// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldReturnMeshValidationAsync()
        {
            // given
            bool outputValidationResult = true;
            bool expectedValidationResult = outputValidationResult;

            this.meshBrokerMock.Setup(broker =>
                broker.HandshakeAsync())
                    .ReturnsAsync(outputValidationResult);

            // when
            bool actualMeshValidationResult =
                await this.meshService.ValidateMailboxAccessAsync();

            // then
            actualMeshValidationResult.Should().Be(expectedValidationResult);

            this.meshBrokerMock.Verify(broker =>
                broker.HandshakeAsync(),
                    Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
