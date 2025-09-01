// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
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
                broker.SelectAllDecisionPollsAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<DecisionPoll>> retrieveAllDecisionPollsTask =
                this.decisionPollService.RetrieveAllDecisionPollsAsync();

            DecisionPollDependencyException actualDecisionPollDependencyException =
                await Assert.ThrowsAsync<DecisionPollDependencyException>(
                    testCode: retrieveAllDecisionPollsTask.AsTask);

            // then
            actualDecisionPollDependencyException.Should()
                .BeEquivalentTo(expectedDecisionPollDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDecisionPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedDecisionPollServiceException =
                new FailedDecisionPollServiceException(
                    message: "Failed decisionPoll service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDecisionPollServiceException =
                new DecisionPollServiceException(
                    message: "DecisionPoll service error occurred, please contact support.",
                    innerException: failedDecisionPollServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDecisionPollsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<DecisionPoll>> retrieveAllDecisionPollsTask =
                this.decisionPollService.RetrieveAllDecisionPollsAsync();

            DecisionPollServiceException actualDecisionPollServiceException =
                await Assert.ThrowsAsync<DecisionPollServiceException>(
                    testCode: retrieveAllDecisionPollsTask.AsTask);

            // then
            actualDecisionPollServiceException.Should()
                .BeEquivalentTo(expectedDecisionPollServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDecisionPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
