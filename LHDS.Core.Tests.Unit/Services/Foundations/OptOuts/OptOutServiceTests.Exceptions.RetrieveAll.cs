using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.OptOuts.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedOptOutStorageException(sqlException);

            var expectedOptOutDependencyException =
                new OptOutDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOuts())
                    .Throws(sqlException);

            // when
            Action retrieveAllOptOutsAction = () =>
                this.optOutService.RetrieveAllOptOuts();

            OptOutDependencyException actualOptOutDependencyException =
                Assert.Throws<OptOutDependencyException>(retrieveAllOptOutsAction);

            // then
            actualOptOutDependencyException.Should()
                .BeEquivalentTo(expectedOptOutDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOptOuts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOptOutDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedOptOutServiceException =
                new FailedOptOutServiceException(serviceException);

            var expectedOptOutServiceException =
                new OptOutServiceException(failedOptOutServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOuts())
                    .Throws(serviceException);

            // when
            Action retrieveAllOptOutsAction = () =>
                this.optOutService.RetrieveAllOptOuts();

            OptOutServiceException actualOptOutServiceException =
                Assert.Throws<OptOutServiceException>(retrieveAllOptOutsAction);

            // then
            actualOptOutServiceException.Should()
                .BeEquivalentTo(expectedOptOutServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOptOuts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}