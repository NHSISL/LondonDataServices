// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
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
                    .Throws(sqlException);

            // when
            Action retrieveAllTerminologyPollsAction = () =>
                this.terminologyPollService.RetrieveAllTerminologyPolls();

            TerminologyPollDependencyException actualTerminologyPollDependencyException =
                Assert.Throws<TerminologyPollDependencyException>(retrieveAllTerminologyPollsAction);

            // then
            actualTerminologyPollDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyPollDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTerminologyPollDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
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
                    .Throws(serviceException);

            // when
            Action retrieveAllTerminologyPollsAction = () =>
                this.terminologyPollService.RetrieveAllTerminologyPolls();

            TerminologyPollServiceException actualTerminologyPollServiceException =
                Assert.Throws<TerminologyPollServiceException>(retrieveAllTerminologyPollsAction);

            // then
            actualTerminologyPollServiceException.Should()
                .BeEquivalentTo(expectedTerminologyPollServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}