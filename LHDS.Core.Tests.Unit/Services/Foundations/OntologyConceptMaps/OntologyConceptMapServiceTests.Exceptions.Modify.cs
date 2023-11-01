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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<OntologyConceptMap> modifyOntologyConceptMapTask =
                this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(randomOntologyConceptMap);

            OntologyConceptMapDependencyException actualOntologyConceptMapDependencyException =
                await Assert.ThrowsAsync<OntologyConceptMapDependencyException>(
                    modifyOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapDependencyException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(randomOntologyConceptMap.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyConceptMapAsync(randomOntologyConceptMap),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            OntologyConceptMap someOntologyConceptMap = CreateRandomOntologyConceptMap();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidOntologyConceptMapReferenceException =
                new InvalidOntologyConceptMapReferenceException(
                    message: "Invalid ontologyConceptMap reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            OntologyConceptMapDependencyValidationException expectedOntologyConceptMapDependencyValidationException =
                new OntologyConceptMapDependencyValidationException(
                    message: "OntologyConceptMap dependency validation occurred, please try again.",
                    innerException: invalidOntologyConceptMapReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<OntologyConceptMap> modifyOntologyConceptMapTask =
                this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(someOntologyConceptMap);

            OntologyConceptMapDependencyValidationException actualOntologyConceptMapDependencyValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapDependencyValidationException>(
                    modifyOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapDependencyValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(someOntologyConceptMap.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedOntologyConceptMapDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyConceptMapAsync(someOntologyConceptMap),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}