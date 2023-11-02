using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();
            SqlException sqlException = GetSqlException();

            var failedTerminologyPollStorageException =
                new FailedTerminologyPollStorageException(
                    message: "Failed terminologyPoll storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedTerminologyPollDependencyException =
                new TerminologyPollDependencyException(
                    message: "TerminologyPoll dependency error occurred, contact support.",
                    innerException: failedTerminologyPollStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<TerminologyPoll> addTerminologyPollTask =
                this.terminologyPollService.AddTerminologyPollAsync(someTerminologyPoll);

            TerminologyPollDependencyException actualTerminologyPollDependencyException =
                await Assert.ThrowsAsync<TerminologyPollDependencyException>(
                    addTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyPollDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTerminologyPollDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfTerminologyPollAlreadyExsitsAndLogItAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll alreadyExistsTerminologyPoll = randomTerminologyPoll;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsTerminologyPollException =
                new AlreadyExistsTerminologyPollException(
                    message: "TerminologyPoll with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedTerminologyPollDependencyValidationException =
                new TerminologyPollDependencyValidationException(
                    message: "TerminologyPoll dependency validation occurred, please try again.",
                    innerException: alreadyExistsTerminologyPollException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<TerminologyPoll> addTerminologyPollTask =
                this.terminologyPollService.AddTerminologyPollAsync(alreadyExistsTerminologyPoll);

            // then
            TerminologyPollDependencyValidationException actualTerminologyPollDependencyValidationException =
                await Assert.ThrowsAsync<TerminologyPollDependencyValidationException>(
                    addTerminologyPollTask.AsTask);

            actualTerminologyPollDependencyValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidTerminologyPollReferenceException =
                new InvalidTerminologyPollReferenceException(
                    message: "Invalid terminologyPoll reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedTerminologyPollValidationException =
                new TerminologyPollDependencyValidationException(
                    message: "TerminologyPoll dependency validation occurred, please try again.",
                    innerException: invalidTerminologyPollReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<TerminologyPoll> addTerminologyPollTask =
                this.terminologyPollService.AddTerminologyPollAsync(someTerminologyPoll);

            // then
            TerminologyPollDependencyValidationException actualTerminologyPollDependencyValidationException =
                await Assert.ThrowsAsync<TerminologyPollDependencyValidationException>(
                    addTerminologyPollTask.AsTask);

            actualTerminologyPollDependencyValidationException.Should().BeEquivalentTo(expectedTerminologyPollValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyPollAsync(someTerminologyPoll),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}