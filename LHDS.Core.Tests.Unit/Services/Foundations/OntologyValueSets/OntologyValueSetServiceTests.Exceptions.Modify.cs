using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.OntologyValueSets;
using LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<OntologyValueSet> modifyOntologyValueSetTask =
                this.ontologyValueSetService.ModifyOntologyValueSetAsync(randomOntologyValueSet);

            OntologyValueSetDependencyException actualOntologyValueSetDependencyException =
                await Assert.ThrowsAsync<OntologyValueSetDependencyException>(
                    modifyOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetDependencyException.Should()
                .BeEquivalentTo(expectedOntologyValueSetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(randomOntologyValueSet.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyValueSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyValueSetAsync(randomOntologyValueSet),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
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

            OntologyValueSetDependencyValidationException expectedOntologyValueSetDependencyValidationException =
                new OntologyValueSetDependencyValidationException(
                    message: "OntologyValueSet dependency validation occurred, please try again.",
                    innerException: invalidOntologyValueSetReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<OntologyValueSet> modifyOntologyValueSetTask =
                this.ontologyValueSetService.ModifyOntologyValueSetAsync(someOntologyValueSet);

            OntologyValueSetDependencyValidationException actualOntologyValueSetDependencyValidationException =
                await Assert.ThrowsAsync<OntologyValueSetDependencyValidationException>(
                    modifyOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetDependencyValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(someOntologyValueSet.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedOntologyValueSetDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyValueSetAsync(someOntologyValueSet),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}