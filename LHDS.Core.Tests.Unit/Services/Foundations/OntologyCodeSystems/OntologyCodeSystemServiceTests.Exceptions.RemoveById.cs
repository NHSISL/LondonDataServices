using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem();
            SqlException sqlException = GetSqlException();

            var failedOntologyCodeSystemStorageException =
                new FailedOntologyCodeSystemStorageException(
                    message: "Failed ontologyCodeSystem storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedOntologyCodeSystemDependencyException =
                new OntologyCodeSystemDependencyException(
                    message: "OntologyCodeSystem dependency error occurred, contact support.",
                    innerException: failedOntologyCodeSystemStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(randomOntologyCodeSystem.Id))
                    .Throws(sqlException);

            // when
            ValueTask<OntologyCodeSystem> addOntologyCodeSystemTask =
                this.ontologyCodeSystemService.RemoveOntologyCodeSystemByIdAsync(randomOntologyCodeSystem.Id);

            OntologyCodeSystemDependencyException actualOntologyCodeSystemDependencyException =
                await Assert.ThrowsAsync<OntologyCodeSystemDependencyException>(
                    addOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemDependencyException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(randomOntologyCodeSystem.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyCodeSystemAsync(It.IsAny<OntologyCodeSystem>()),
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
            Guid someOntologyCodeSystemId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedOntologyCodeSystemException =
                new LockedOntologyCodeSystemException(
                    message: "Locked ontologyCodeSystem record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedOntologyCodeSystemDependencyValidationException =
                new OntologyCodeSystemDependencyValidationException(
                    message: "OntologyCodeSystem dependency validation occurred, please try again.",
                    innerException: lockedOntologyCodeSystemException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<OntologyCodeSystem> removeOntologyCodeSystemByIdTask =
                this.ontologyCodeSystemService.RemoveOntologyCodeSystemByIdAsync(someOntologyCodeSystemId);

            OntologyCodeSystemDependencyValidationException actualOntologyCodeSystemDependencyValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemDependencyValidationException>(
                    removeOntologyCodeSystemByIdTask.AsTask);

            // then
            actualOntologyCodeSystemDependencyValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyCodeSystemAsync(It.IsAny<OntologyCodeSystem>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someOntologyCodeSystemId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedOntologyCodeSystemStorageException =
                new FailedOntologyCodeSystemStorageException(
                    message: "Failed ontologyCodeSystem storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedOntologyCodeSystemDependencyException =
                new OntologyCodeSystemDependencyException(
                    message: "OntologyCodeSystem dependency error occurred, contact support.",
                    innerException: failedOntologyCodeSystemStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<OntologyCodeSystem> deleteOntologyCodeSystemTask =
                this.ontologyCodeSystemService.RemoveOntologyCodeSystemByIdAsync(someOntologyCodeSystemId);

            OntologyCodeSystemDependencyException actualOntologyCodeSystemDependencyException =
                await Assert.ThrowsAsync<OntologyCodeSystemDependencyException>(
                    deleteOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemDependencyException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someOntologyCodeSystemId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedOntologyCodeSystemServiceException =
                new FailedOntologyCodeSystemServiceException(
                    message: "Failed ontologyCodeSystem service occurred, please contact support", 
                    innerException: serviceException);

            var expectedOntologyCodeSystemServiceException =
                new OntologyCodeSystemServiceException(
                    message: "OntologyCodeSystem service error occurred, contact support.",
                    innerException: failedOntologyCodeSystemServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<OntologyCodeSystem> removeOntologyCodeSystemByIdTask =
                this.ontologyCodeSystemService.RemoveOntologyCodeSystemByIdAsync(someOntologyCodeSystemId);

            OntologyCodeSystemServiceException actualOntologyCodeSystemServiceException =
                await Assert.ThrowsAsync<OntologyCodeSystemServiceException>(
                    removeOntologyCodeSystemByIdTask.AsTask);

            // then
            actualOntologyCodeSystemServiceException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}