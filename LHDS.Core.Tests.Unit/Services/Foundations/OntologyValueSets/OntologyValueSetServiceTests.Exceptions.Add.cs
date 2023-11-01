using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            OntologyValueSet someOntologyValueSet = CreateRandomOntologyValueSet();
            SqlException sqlException = GetSqlException();

            var failedOntologyValueSetStorageException =
                new FailedOntologyValueSetStorageException(
                    message: "Failed ontologyValueSet storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedOntologyValueSetDependencyException =
                new OntologyValueSetDependencyException(
                    message: "OntologyValueSet dependency error occurred, contact support.",
                    innerException: failedOntologyValueSetStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<OntologyValueSet> addOntologyValueSetTask =
                this.ontologyValueSetService.AddOntologyValueSetAsync(someOntologyValueSet);

            OntologyValueSetDependencyException actualOntologyValueSetDependencyException =
                await Assert.ThrowsAsync<OntologyValueSetDependencyException>(
                    addOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetDependencyException.Should()
                .BeEquivalentTo(expectedOntologyValueSetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyValueSetAsync(It.IsAny<OntologyValueSet>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyValueSetDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfOntologyValueSetAlreadyExsitsAndLogItAsync()
        {
            // given
            OntologyValueSet randomOntologyValueSet = CreateRandomOntologyValueSet();
            OntologyValueSet alreadyExistsOntologyValueSet = randomOntologyValueSet;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsOntologyValueSetException =
                new AlreadyExistsOntologyValueSetException(
                    message: "OntologyValueSet with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedOntologyValueSetDependencyValidationException =
                new OntologyValueSetDependencyValidationException(
                    message: "OntologyValueSet dependency validation occurred, please try again.",
                    innerException: alreadyExistsOntologyValueSetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<OntologyValueSet> addOntologyValueSetTask =
                this.ontologyValueSetService.AddOntologyValueSetAsync(alreadyExistsOntologyValueSet);

            // then
            OntologyValueSetDependencyValidationException actualOntologyValueSetDependencyValidationException =
                await Assert.ThrowsAsync<OntologyValueSetDependencyValidationException>(
                    addOntologyValueSetTask.AsTask);

            actualOntologyValueSetDependencyValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyValueSetAsync(It.IsAny<OntologyValueSet>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            OntologyValueSet someOntologyValueSet = CreateRandomOntologyValueSet();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidOntologyValueSetReferenceException =
                new InvalidOntologyValueSetReferenceException(
                    message: "Invalid ontologyValueSet reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedOntologyValueSetValidationException =
                new OntologyValueSetDependencyValidationException(
                    message: "OntologyValueSet dependency validation occurred, please try again.",
                    innerException: invalidOntologyValueSetReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<OntologyValueSet> addOntologyValueSetTask =
                this.ontologyValueSetService.AddOntologyValueSetAsync(someOntologyValueSet);

            // then
            OntologyValueSetDependencyValidationException actualOntologyValueSetDependencyValidationException =
                await Assert.ThrowsAsync<OntologyValueSetDependencyValidationException>(
                    addOntologyValueSetTask.AsTask);

            actualOntologyValueSetDependencyValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyValueSetAsync(someOntologyValueSet),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            OntologyValueSet someOntologyValueSet = CreateRandomOntologyValueSet();

            var databaseUpdateException =
                new DbUpdateException();

            var failedOntologyValueSetStorageException =
                new FailedOntologyValueSetStorageException(
                    message: "Failed ontologyValueSet storage error occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedOntologyValueSetDependencyException =
                new OntologyValueSetDependencyException(
                    message: "OntologyValueSet dependency error occurred, contact support.",
                    innerException: failedOntologyValueSetStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<OntologyValueSet> addOntologyValueSetTask =
                this.ontologyValueSetService.AddOntologyValueSetAsync(someOntologyValueSet);

            OntologyValueSetDependencyException actualOntologyValueSetDependencyException =
                await Assert.ThrowsAsync<OntologyValueSetDependencyException>(
                    addOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetDependencyException.Should()
                .BeEquivalentTo(expectedOntologyValueSetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyValueSetAsync(It.IsAny<OntologyValueSet>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}