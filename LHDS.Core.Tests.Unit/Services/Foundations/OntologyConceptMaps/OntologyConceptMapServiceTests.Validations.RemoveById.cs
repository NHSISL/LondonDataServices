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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidOntologyConceptMapId = Guid.Empty;

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
            ValueTask<OntologyConceptMap> removeOntologyConceptMapByIdTask =
                this.ontologyConceptMapService.RemoveOntologyConceptMapByIdAsync(invalidOntologyConceptMapId);

            OntologyConceptMapValidationException actualOntologyConceptMapValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                    removeOntologyConceptMapByIdTask.AsTask);

            // then
            actualOntologyConceptMapValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyConceptMapAsync(It.IsAny<OntologyConceptMap>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}