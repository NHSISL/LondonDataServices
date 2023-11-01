using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            OntologyConceptMap someOntologyConceptMap = CreateRandomOntologyConceptMap();
            SqlException sqlException = GetSqlException();

            var failedOntologyConceptMapStorageException =
                new FailedOntologyConceptMapStorageException(
                    message: "Failed ontologyConceptMap storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedOntologyConceptMapDependencyException =
                new OntologyConceptMapDependencyException(
                    message: "OntologyConceptMap dependency error occurred, contact support.",
                    innerException: failedOntologyConceptMapStorageException);             

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<OntologyConceptMap> addOntologyConceptMapTask =
                this.ontologyConceptMapService.AddOntologyConceptMapAsync(someOntologyConceptMap);

            OntologyConceptMapDependencyException actualOntologyConceptMapDependencyException =
                await Assert.ThrowsAsync<OntologyConceptMapDependencyException>(
                    addOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapDependencyException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyConceptMapAsync(It.IsAny<OntologyConceptMap>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfOntologyConceptMapAlreadyExsitsAndLogItAsync()
        {
            // given
            OntologyConceptMap randomOntologyConceptMap = CreateRandomOntologyConceptMap();
            OntologyConceptMap alreadyExistsOntologyConceptMap = randomOntologyConceptMap;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsOntologyConceptMapException =
                new AlreadyExistsOntologyConceptMapException(
                    message: "OntologyConceptMap with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedOntologyConceptMapDependencyValidationException =
                new OntologyConceptMapDependencyValidationException(
                    message: "OntologyConceptMap dependency validation occurred, please try again.",
                    innerException: alreadyExistsOntologyConceptMapException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<OntologyConceptMap> addOntologyConceptMapTask =
                this.ontologyConceptMapService.AddOntologyConceptMapAsync(alreadyExistsOntologyConceptMap);

            // then
            OntologyConceptMapDependencyValidationException actualOntologyConceptMapDependencyValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapDependencyValidationException>(
                    addOntologyConceptMapTask.AsTask);

            actualOntologyConceptMapDependencyValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyConceptMapAsync(It.IsAny<OntologyConceptMap>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}