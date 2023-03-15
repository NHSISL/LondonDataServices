// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.MeshItems.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAcknowledgeMessageByIdIfArgsIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string param1 = invalidText;
            string param2 = invalidText;

            var invalidArgumentMeshException =
                new InvalidArgumentMeshException();

            invalidArgumentMeshException.AddData(
                key: nameof(param1),
                values: "Param1 is required");

            invalidArgumentMeshException.AddData(
              key: nameof(param2),
              values: "Param2 is required");

            var expectedMeshValidationException =
                new MeshValidationException(invalidArgumentMeshException);

            // when
            ValueTask<bool> retrieveAknowledgeMessageByIdTask =
                this.meshService.AcknowledgeMessageByIdAsync(param1, param2);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(
                    retrieveAknowledgeMessageByIdTask.AsTask);

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMeshValidationException))),
                        Times.Once);

            this.meshBrokerMock.Verify(broker =>
               broker.AcknowledgeMessageByIdAsync(It.IsAny<string>(), It.IsAny<string>()),
                   Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}