using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyArtifactIsNullAndLogItAsync()
        {
            // given
            TerminologyArtifact nullTerminologyArtifact = null;
            var nullTerminologyArtifactException = new NullTerminologyArtifactException(message: "TerminologyArtifact is null.");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: nullTerminologyArtifactException);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(nullTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}