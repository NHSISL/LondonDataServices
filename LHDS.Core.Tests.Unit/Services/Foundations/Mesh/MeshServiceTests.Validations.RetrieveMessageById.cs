// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading;
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

            var invalidMeshMessageException =
                new InvalidMeshMessageException(
                    message: "Invalid mesh message, please correct errors and try again.");

            invalidMeshMessageException.AddData(
                key: "MessageId",
                values: "Text is required");

            var expectedMeshValidationException =
                new MeshValidationException(
                    message: "Mesh validation errors occurred, please try again.",
                    innerException: invalidMeshMessageException);

            // when
            ValueTask<MeshMessage> retrieveTrackingStatusTask =
                this.meshService.RetrieveMessageByIdAsync(
                    messageId,
                    outputStream: Stream.Null,
                    TestContext.Current.CancellationToken);

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
               broker.RetrieveMessageAsync(
                    messageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                   Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
