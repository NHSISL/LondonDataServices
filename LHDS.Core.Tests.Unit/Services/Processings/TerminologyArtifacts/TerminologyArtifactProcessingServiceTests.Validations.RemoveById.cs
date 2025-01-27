// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRemoveIfIdIsNullAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;

            var invalidArgumentTerminologyArtifactProcessingException =
                new InvalidArgumentTerminologyArtifactProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentTerminologyArtifactProcessingException.AddData(
                key: "Id",
                values: "Id is required");

            var expectedTerminologyArtifactProcessingValidationException =
                new TerminologyArtifactProcessingValidationException(
                    message: "Terminology artifact processing validation error occurred, please try again.",
                    innerException: invalidArgumentTerminologyArtifactProcessingException);

            // when
            ValueTask<TerminologyArtifact> RemoveTerminologyArtifactTask =
                this.terminologyArtifactProcessingService.RemoveTerminologyArtifactByIdAsync(invalidId);

            TerminologyArtifactProcessingValidationException actualTerminologyArtifactProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingValidationException>(
                    RemoveTerminologyArtifactTask.AsTask);

            //then
            actualTerminologyArtifactProcessingValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactProcessingValidationException))),
                        Times.Once);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
