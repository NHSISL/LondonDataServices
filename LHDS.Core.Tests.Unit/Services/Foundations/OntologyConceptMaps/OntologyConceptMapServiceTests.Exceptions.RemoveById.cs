using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someOntologyConceptMapId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedOntologyConceptMapException =
                new LockedOntologyConceptMapException(
                    message: "Locked ontologyConceptMap record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedOntologyConceptMapDependencyValidationException =
                new OntologyConceptMapDependencyValidationException(
                    message: "OntologyConceptMap dependency validation occurred, please try again.",
                    innerException: lockedOntologyConceptMapException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyConceptMapByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<OntologyConceptMap> removeOntologyConceptMapByIdTask =
                this.ontologyConceptMapService.RemoveOntologyConceptMapByIdAsync(someOntologyConceptMapId);

            OntologyConceptMapDependencyValidationException actualOntologyConceptMapDependencyValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapDependencyValidationException>(
                    removeOntologyConceptMapByIdTask.AsTask);

            // then
            actualOntologyConceptMapDependencyValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyConceptMapAsync(It.IsAny<OntologyConceptMap>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someOntologyConceptMapId = Guid.NewGuid();
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
                broker.SelectOntologyConceptMapByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<OntologyConceptMap> deleteOntologyConceptMapTask =
                this.ontologyConceptMapService.RemoveOntologyConceptMapByIdAsync(someOntologyConceptMapId);

            OntologyConceptMapDependencyException actualOntologyConceptMapDependencyException =
                await Assert.ThrowsAsync<OntologyConceptMapDependencyException>(
                    deleteOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapDependencyException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}