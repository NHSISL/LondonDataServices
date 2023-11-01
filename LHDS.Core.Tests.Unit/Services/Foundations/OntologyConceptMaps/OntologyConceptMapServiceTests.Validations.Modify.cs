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
        public async Task ShouldThrowValidationExceptionOnModifyIfOntologyConceptMapIsNullAndLogItAsync()
        {
            // given
            OntologyConceptMap nullOntologyConceptMap = null;
            var nullOntologyConceptMapException = new NullOntologyConceptMapException(message: "OntologyConceptMap is null.");

            var expectedOntologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: nullOntologyConceptMapException);

            // when
            ValueTask<OntologyConceptMap> modifyOntologyConceptMapTask =
                this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(nullOntologyConceptMap);

            OntologyConceptMapValidationException actualOntologyConceptMapValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                    modifyOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyConceptMapAsync(It.IsAny<OntologyConceptMap>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}