// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedTerminologyPollStorageException =
                new FailedTerminologyPollStorageException(
                    message: "Failed terminologyPoll storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedTerminologyPollDependencyException =
                new TerminologyPollDependencyException(
                    message: "TerminologyPoll dependency error occurred, please contact support.",
                    innerException: failedTerminologyPollStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyPollsAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<TerminologyPoll>> retrieveAllTerminologyPollsTask =
                this.terminologyPollService.RetrieveAllTerminologyPollsAsync();

            TerminologyPollDependencyException actualTerminologyPollDependencyException =
                await Assert.ThrowsAsync<TerminologyPollDependencyException>(
                    testCode: retrieveAllTerminologyPollsTask.AsTask);

            // then
            actualTerminologyPollDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyPollDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollDependencyException))),
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

            var failedTerminologyPollServiceException =
                new FailedTerminologyPollServiceException(
                    message: "Failed terminologyPoll service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedTerminologyPollServiceException =
                new TerminologyPollServiceException(
                    message: "TerminologyPoll service error occurred, please contact support.",
                    innerException: failedTerminologyPollServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyPollsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<TerminologyPoll>> retrieveAllTerminologyPollsTask =
                this.terminologyPollService.RetrieveAllTerminologyPollsAsync();

            TerminologyPollServiceException actualTerminologyPollServiceException =
                await Assert.ThrowsAsync<TerminologyPollServiceException>(
                    testCode: retrieveAllTerminologyPollsTask.AsTask);

            // then
            actualTerminologyPollServiceException.Should()
                .BeEquivalentTo(expectedTerminologyPollServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}