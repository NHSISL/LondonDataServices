using System.Threading.Tasks;
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
    }
}