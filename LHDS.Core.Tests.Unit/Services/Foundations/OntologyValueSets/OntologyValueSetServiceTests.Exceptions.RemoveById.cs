using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Foundations.OntologyValueSets;
using LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            OntologyValueSet randomOntologyValueSet = CreateRandomOntologyValueSet();
            SqlException sqlException = GetSqlException();

            var failedOntologyValueSetStorageException =
                new FailedOntologyValueSetStorageException(
                    message: "Failed ontologyValueSet storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedOntologyValueSetDependencyException =
                new OntologyValueSetDependencyException(
                    message: "OntologyValueSet dependency error occurred, contact support.",
                    innerException: failedOntologyValueSetStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyValueSetByIdAsync(randomOntologyValueSet.Id))
                    .Throws(sqlException);

            // when
            ValueTask<OntologyValueSet> addOntologyValueSetTask =
                this.ontologyValueSetService.RemoveOntologyValueSetByIdAsync(randomOntologyValueSet.Id);

            OntologyValueSetDependencyException actualOntologyValueSetDependencyException =
                await Assert.ThrowsAsync<OntologyValueSetDependencyException>(
                    addOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetDependencyException.Should()
                .BeEquivalentTo(expectedOntologyValueSetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(randomOntologyValueSet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyValueSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyValueSetAsync(It.IsAny<OntologyValueSet>()),
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
            Guid someOntologyValueSetId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedOntologyValueSetException =
                new LockedOntologyValueSetException(
                    message: "Locked ontologyValueSet record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedOntologyValueSetDependencyValidationException =
                new OntologyValueSetDependencyValidationException(
                    message: "OntologyValueSet dependency validation occurred, please try again.",
                    innerException: lockedOntologyValueSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<OntologyValueSet> removeOntologyValueSetByIdTask =
                this.ontologyValueSetService.RemoveOntologyValueSetByIdAsync(someOntologyValueSetId);

            OntologyValueSetDependencyValidationException actualOntologyValueSetDependencyValidationException =
                await Assert.ThrowsAsync<OntologyValueSetDependencyValidationException>(
                    removeOntologyValueSetByIdTask.AsTask);

            // then
            actualOntologyValueSetDependencyValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyValueSetAsync(It.IsAny<OntologyValueSet>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someOntologyValueSetId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedOntologyValueSetStorageException =
                new FailedOntologyValueSetStorageException(
                    message: "Failed ontologyValueSet storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedOntologyValueSetDependencyException =
                new OntologyValueSetDependencyException(
                    message: "OntologyValueSet dependency error occurred, contact support.",
                    innerException: failedOntologyValueSetStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<OntologyValueSet> deleteOntologyValueSetTask =
                this.ontologyValueSetService.RemoveOntologyValueSetByIdAsync(someOntologyValueSetId);

            OntologyValueSetDependencyException actualOntologyValueSetDependencyException =
                await Assert.ThrowsAsync<OntologyValueSetDependencyException>(
                    deleteOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetDependencyException.Should()
                .BeEquivalentTo(expectedOntologyValueSetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyValueSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someOntologyValueSetId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedOntologyValueSetServiceException =
                new FailedOntologyValueSetServiceException(
                    message: "Failed ontologyValueSet service occurred, please contact support", 
                    innerException: serviceException);

            var expectedOntologyValueSetServiceException =
                new OntologyValueSetServiceException(
                    message: "OntologyValueSet service error occurred, contact support.",
                    innerException: failedOntologyValueSetServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<OntologyValueSet> removeOntologyValueSetByIdTask =
                this.ontologyValueSetService.RemoveOntologyValueSetByIdAsync(someOntologyValueSetId);

            OntologyValueSetServiceException actualOntologyValueSetServiceException =
                await Assert.ThrowsAsync<OntologyValueSetServiceException>(
                    removeOntologyValueSetByIdTask.AsTask);

            // then
            actualOntologyValueSetServiceException.Should()
                .BeEquivalentTo(expectedOntologyValueSetServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}