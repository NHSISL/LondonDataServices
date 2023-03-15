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
        public async Task ShouldThrowValidationExceptionOnRetrieveTrackingStatusIfArgsIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string mailboxId = invalidText;
            string messageId = invalidText;

            var invalidArgumentMeshException =
                new InvalidArgumentMeshException();

            invalidArgumentMeshException.AddData(
                key: nameof(mailboxId),
                values: "Text is required");

            invalidArgumentMeshException.AddData(
                key: nameof(messageId),
                values: "Text is required");

            var expectedMeshValidationException =
                new MeshValidationException(
                    innerException: invalidArgumentMeshException,
                    validationSummary: GetValidationSummary(invalidArgumentMeshException.Data));

            // when
            ValueTask<string> retrieveTrackingStatusTask =
                this.meshService.RetrieveTrackingStatusAsync(mailboxId, messageId);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(
                    retrieveTrackingStatusTask.AsTask);

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMeshValidationException))),
                        Times.Once);

            this.meshBrokerMock.Verify(broker =>
               broker.GetTrackingStatusAsync(mailboxId, messageId),
                   Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
