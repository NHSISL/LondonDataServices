using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidOntologyConceptMapId = Guid.Empty;

            var invalidOntologyConceptMapException = 
                new InvalidOntologyConceptMapException(
                    message: "Invalid ontologyConceptMap. Please correct the errors and try again.");

            invalidOntologyConceptMapException.AddData(
                key: nameof(OntologyConceptMap.Id),
                values: "Id is required");

            var expectedOntologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: invalidOntologyConceptMapException);

            // when
            ValueTask<OntologyConceptMap> retrieveOntologyConceptMapByIdTask =
                this.ontologyConceptMapService.RetrieveOntologyConceptMapByIdAsync(invalidOntologyConceptMapId);

            OntologyConceptMapValidationException actualOntologyConceptMapValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                    retrieveOntologyConceptMapByIdTask.AsTask);

            // then
            actualOntologyConceptMapValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfOntologyConceptMapIsNotFoundAndLogItAsync()
        {
            //given
            Guid someOntologyConceptMapId = Guid.NewGuid();
            OntologyConceptMap noOntologyConceptMap = null;

            var notFoundOntologyConceptMapException =
                new NotFoundOntologyConceptMapException(someOntologyConceptMapId);

            var expectedOntologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: notFoundOntologyConceptMapException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyConceptMapByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noOntologyConceptMap);

            //when
            ValueTask<OntologyConceptMap> retrieveOntologyConceptMapByIdTask =
                this.ontologyConceptMapService.RetrieveOntologyConceptMapByIdAsync(someOntologyConceptMapId);

            OntologyConceptMapValidationException actualOntologyConceptMapValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                    retrieveOntologyConceptMapByIdTask.AsTask);

            //then
            actualOntologyConceptMapValidationException.Should().BeEquivalentTo(expectedOntologyConceptMapValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}