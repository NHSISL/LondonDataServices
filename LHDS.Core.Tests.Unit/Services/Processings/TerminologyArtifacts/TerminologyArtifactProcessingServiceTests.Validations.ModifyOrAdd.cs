// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task
            ShouldThrowValidationExceptionsOnModifyOrAddIfTerminologyArtifactProcessingIsNullAndLogItAsync()
        {
            // given
            TerminologyArtifact nullTerminologyArtifact = null;

            var nullTerminologyArtifactProcessingException =
                new NullTerminologyArtifactProcessingException(message: "Terminology artifact is null.");

            var expectedTerminologyArtifactProcessingValidationException =
                new TerminologyArtifactProcessingValidationException(
                    message: "Terminology artifact processing validation error occurred, please try again.",
                    innerException: nullTerminologyArtifactProcessingException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactProcessingService.ModifyOrAddTerminologyArtifactAsync(nullTerminologyArtifact);

            TerminologyArtifactProcessingValidationException actualTerminologyArtifactProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingValidationException>(
                    addTerminologyArtifactTask.AsTask);

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

        [Fact]
        public async Task ShouldThrowValidationExceptionsOnModifyOrAddIfDataSetDoesNotHaveValidIdAndLogItAsync()
        {
            // given
            TerminologyArtifact emptyTerminologyArtifact = new TerminologyArtifact();

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
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactProcessingService.ModifyOrAddTerminologyArtifactAsync(emptyTerminologyArtifact);

            TerminologyArtifactProcessingValidationException actualTerminologyArtifactProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingValidationException>(
                    addTerminologyArtifactTask.AsTask);

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
