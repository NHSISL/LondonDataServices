using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<OntologyCodeSystem> modifyOntologyCodeSystemTask =
                this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(randomOntologyCodeSystem);

            OntologyCodeSystemDependencyException actualOntologyCodeSystemDependencyException =
                await Assert.ThrowsAsync<OntologyCodeSystemDependencyException>(
                    modifyOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemDependencyException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(randomOntologyCodeSystem.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyCodeSystemAsync(randomOntologyCodeSystem),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            OntologyCodeSystem someOntologyCodeSystem = CreateRandomOntologyCodeSystem();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidOntologyCodeSystemReferenceException =
                new InvalidOntologyCodeSystemReferenceException(
                    message: "Invalid ontologyCodeSystem reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            OntologyCodeSystemDependencyValidationException expectedOntologyCodeSystemDependencyValidationException =
                new OntologyCodeSystemDependencyValidationException(
                    message: "OntologyCodeSystem dependency validation occurred, please try again.",
                    innerException: invalidOntologyCodeSystemReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<OntologyCodeSystem> modifyOntologyCodeSystemTask =
                this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(someOntologyCodeSystem);

            OntologyCodeSystemDependencyValidationException actualOntologyCodeSystemDependencyValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemDependencyValidationException>(
                    modifyOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemDependencyValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(someOntologyCodeSystem.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedOntologyCodeSystemDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyCodeSystemAsync(someOntologyCodeSystem),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem();
            var databaseUpdateException = new DbUpdateException();

            var failedOntologyCodeSystemStorageException =
                new FailedOntologyCodeSystemStorageException(
                    message: "Failed ontologyCodeSystem storage error occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedOntologyCodeSystemDependencyException =
                new OntologyCodeSystemDependencyException(
                    message: "OntologyCodeSystem dependency error occurred, contact support.",
                    innerException: failedOntologyCodeSystemStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<OntologyCodeSystem> modifyOntologyCodeSystemTask =
                this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(randomOntologyCodeSystem);

            OntologyCodeSystemDependencyException actualOntologyCodeSystemDependencyException =
                await Assert.ThrowsAsync<OntologyCodeSystemDependencyException>(
                    modifyOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemDependencyException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(randomOntologyCodeSystem.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyCodeSystemAsync(randomOntologyCodeSystem),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedOntologyCodeSystemException =
                new LockedOntologyCodeSystemException(
                    message: "Locked ontologyCodeSystem record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedOntologyCodeSystemDependencyValidationException =
                new OntologyCodeSystemDependencyValidationException(
                    message: "OntologyCodeSystem dependency validation occurred, please try again.",
                    innerException: lockedOntologyCodeSystemException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<OntologyCodeSystem> modifyOntologyCodeSystemTask =
                this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(randomOntologyCodeSystem);

            OntologyCodeSystemDependencyValidationException actualOntologyCodeSystemDependencyValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemDependencyValidationException>(
                    modifyOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemDependencyValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(randomOntologyCodeSystem.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyCodeSystemAsync(randomOntologyCodeSystem),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem();
            var serviceException = new Exception();

            var failedOntologyCodeSystemServiceException =
                new FailedOntologyCodeSystemServiceException(
                    message: "Failed ontologyCodeSystem service occurred, please contact support", 
                    innerException: serviceException);

            var expectedOntologyCodeSystemServiceException =
                new OntologyCodeSystemServiceException(
                    message: "OntologyCodeSystem service error occurred, contact support.",
                    innerException: failedOntologyCodeSystemServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<OntologyCodeSystem> modifyOntologyCodeSystemTask =
                this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(randomOntologyCodeSystem);

            OntologyCodeSystemServiceException actualOntologyCodeSystemServiceException =
                await Assert.ThrowsAsync<OntologyCodeSystemServiceException>(
                    modifyOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemServiceException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(randomOntologyCodeSystem.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyCodeSystemAsync(randomOntologyCodeSystem),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}