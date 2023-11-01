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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidOntologyValueSetId = Guid.Empty;

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
            ValueTask<OntologyValueSet> retrieveOntologyValueSetByIdTask =
                this.ontologyValueSetService.RetrieveOntologyValueSetByIdAsync(invalidOntologyValueSetId);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    retrieveOntologyValueSetByIdTask.AsTask);

            // then
            actualOntologyValueSetValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfOntologyValueSetIsNotFoundAndLogItAsync()
        {
            //given
            Guid someOntologyValueSetId = Guid.NewGuid();
            OntologyValueSet noOntologyValueSet = null;

            var notFoundOntologyValueSetException =
                new NotFoundOntologyValueSetException(someOntologyValueSetId);

            var expectedOntologyValueSetValidationException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: notFoundOntologyValueSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noOntologyValueSet);

            //when
            ValueTask<OntologyValueSet> retrieveOntologyValueSetByIdTask =
                this.ontologyValueSetService.RetrieveOntologyValueSetByIdAsync(someOntologyValueSetId);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    retrieveOntologyValueSetByIdTask.AsTask);

            //then
            actualOntologyValueSetValidationException.Should().BeEquivalentTo(expectedOntologyValueSetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}