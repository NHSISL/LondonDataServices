using System;
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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidOntologyValueSetId = Guid.Empty;

            var invalidOntologyValueSetException = 
                new InvalidOntologyValueSetException(
                    message: "Invalid ontologyValueSet. Please correct the errors and try again.");

            invalidOntologyValueSetException.AddData(
                key: nameof(OntologyValueSet.Id),
                values: "Id is required");

            var expectedOntologyValueSetValidationException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: invalidOntologyValueSetException);

            // when
            ValueTask<OntologyValueSet> removeOntologyValueSetByIdTask =
                this.ontologyValueSetService.RemoveOntologyValueSetByIdAsync(invalidOntologyValueSetId);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    removeOntologyValueSetByIdTask.AsTask);

            // then
            actualOntologyValueSetValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyValueSetAsync(It.IsAny<OntologyValueSet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}