// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
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
        public async Task ShouldThrowValidationExceptionOnRetrieveMessageByIdIfArgsIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string messageId = invalidText;

            var invalidArgumentMeshException =
                new InvalidArgumentMeshException(
                    message: "Invalid Mesh argument(s), please correct the errors and try again.");

            invalidArgumentMeshException.AddData(
                key: "MessageId",
                values: "Text is required");

            var expectedMeshValidationException =
                new MeshValidationException(
                    message: "Mesh validation errors occurred, please try again.",
                    innerException: invalidArgumentMeshException);

            // when
            ValueTask<MeshMessage> retrieveTrackingStatusTask =
                this.meshService.RetrieveMessageByIdAsync(messageId);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(
                    retrieveTrackingStatusTask.AsTask);

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedMeshValidationException))),
                        Times.Once);

            this.meshBrokerMock.Verify(broker =>
               broker.RetrieveMessageAsync(messageId),
                   Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
