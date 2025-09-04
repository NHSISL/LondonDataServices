// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.DecisionPolls.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll();
            SqlException sqlException = GetSqlException();

            var failedDecisionPollStorageException =
                new FailedDecisionPollStorageException(
                    message: "Failed decisionPoll storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDecisionPollDependencyException =
                new DecisionPollDependencyException(
                    message: "DecisionPoll dependency error occurred, please contact support.",
                    innerException: failedDecisionPollStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(randomDecisionPoll.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DecisionPoll> addDecisionPollTask =
                this.decisionPollService.RemoveDecisionPollByIdAsync(randomDecisionPoll.Id);

            DecisionPollDependencyException actualDecisionPollDependencyException =
                await Assert.ThrowsAsync<DecisionPollDependencyException>(
                    addDecisionPollTask.AsTask);

            // then
            actualDecisionPollDependencyException.Should()
                .BeEquivalentTo(expectedDecisionPollDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(randomDecisionPoll.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
