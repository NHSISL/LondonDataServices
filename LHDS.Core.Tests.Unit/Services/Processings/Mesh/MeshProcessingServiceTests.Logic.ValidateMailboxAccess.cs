// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldReturnMeshValidationAsync()
        {
            // given
            bool outputValidationResult = true;
            bool expectedValidationResult = outputValidationResult;

            // when
            bool actualMeshValidationResult =
                await this.meshProcessingService.ValidateMailboxAccessAsync();

            // then
            actualMeshValidationResult.Should().Be(expectedValidationResult);

            this.meshServiceMock.Verify(service =>
                service.ValidateMailboxAccessAsync(),
                    Times.Once());

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
