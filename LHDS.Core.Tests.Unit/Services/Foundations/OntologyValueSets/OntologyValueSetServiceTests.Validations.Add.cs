using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.OntologyValueSets;
using LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfOntologyValueSetIsNullAndLogItAsync()
        {
            // given
            OntologyValueSet nullOntologyValueSet = null;

            var nullOntologyValueSetException =
                new NullOntologyValueSetException(message: "OntologyValueSet is null.");

            var expectedOntologyValueSetValidationException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: nullOntologyValueSetException);

            // when
            ValueTask<OntologyValueSet> addOntologyValueSetTask =
                this.ontologyValueSetService.AddOntologyValueSetAsync(nullOntologyValueSet);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    addOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetValidationException.Should().BeEquivalentTo(expectedOntologyValueSetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}