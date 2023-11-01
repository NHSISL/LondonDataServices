using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            OntologyConceptMap randomOntologyConceptMap = CreateRandomOntologyConceptMap();
            SqlException sqlException = GetSqlException();

            var failedOntologyConceptMapStorageException =
                new FailedOntologyConceptMapStorageException(
                    message: "Failed ontologyConceptMap storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedOntologyConceptMapDependencyException =
                new OntologyConceptMapDependencyException(
                    message: "OntologyConceptMap dependency error occurred, contact support.",
                    innerException: failedOntologyConceptMapStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyConceptMapByIdAsync(randomOntologyConceptMap.Id))
                    .Throws(sqlException);

            // when
            ValueTask<OntologyConceptMap> addOntologyConceptMapTask =
                this.ontologyConceptMapService.RemoveOntologyConceptMapByIdAsync(randomOntologyConceptMap.Id);

            OntologyConceptMapDependencyException actualOntologyConceptMapDependencyException =
                await Assert.ThrowsAsync<OntologyConceptMapDependencyException>(
                    addOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapDependencyException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(randomOntologyConceptMap.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyConceptMapAsync(It.IsAny<OntologyConceptMap>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}