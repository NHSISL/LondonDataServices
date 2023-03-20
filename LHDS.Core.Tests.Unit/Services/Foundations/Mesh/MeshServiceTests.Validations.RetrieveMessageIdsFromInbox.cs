// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowValidationExceptionOnRetrieveMessageIdsFromInboxIfArgsIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string mailboxId = invalidText;

            var invalidArgumentMeshException =
                new InvalidArgumentMeshException();

            invalidArgumentMeshException.AddData(
                key: nameof(mailboxId),
                values: "Text is required");

            var expectedMeshValidationException =
                new MeshValidationException(
                    innerException: invalidArgumentMeshException,
                    validationSummary: GetValidationSummary(invalidArgumentMeshException.Data));

            // when
            ValueTask<List<string>> retrieveMessageIdsFromInboxTask =
                this.meshService.RetrieveMessageIdsFromInboxAsync(mailboxId);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(
                    retrieveMessageIdsFromInboxTask.AsTask);

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMeshValidationException))),
                        Times.Once);

            this.meshBrokerMock.Verify(broker =>
               broker.GetMessageIdsFromInboxAsync(mailboxId),
                   Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
