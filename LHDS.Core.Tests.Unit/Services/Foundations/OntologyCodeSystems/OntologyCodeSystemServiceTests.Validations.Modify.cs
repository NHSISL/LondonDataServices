using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOntologyCodeSystemIsNullAndLogItAsync()
        {
            // given
            OntologyCodeSystem nullOntologyCodeSystem = null;
            var nullOntologyCodeSystemException = new NullOntologyCodeSystemException(message: "OntologyCodeSystem is null.");

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: nullOntologyCodeSystemException);

            // when
            ValueTask<OntologyCodeSystem> modifyOntologyCodeSystemTask =
                this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(nullOntologyCodeSystem);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(
                    modifyOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyCodeSystemAsync(It.IsAny<OntologyCodeSystem>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}