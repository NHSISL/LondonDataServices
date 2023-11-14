// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Processings.TerminologyArtifact.Exceptions;
using LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnModifyOrAddIfTerminologyArtifactProcessingIsNullAndLogItAsync()
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
            ValueTask<TerminologyArtifact> AddTerminologyArtifactTask =
                this.terminologyArtifactProcessingService.ModifyOrAddTerminologyArtifactAsync(nullTerminologyArtifact);

            TerminologyArtifactProcessingValidationException actualTerminologyArtifactProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingValidationException>(
                    AddTerminologyArtifactTask.AsTask);

            //then
            actualTerminologyArtifactProcessingValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactProcessingValidationException))),
                        Times.Once);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
