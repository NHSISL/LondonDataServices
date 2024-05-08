// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
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

            var failedOptOutStorageException = new FailedOptOutStorageException(
                message: "Failed optOut storage error occurred, please contact support.",
                innerException: sqlException);

            var expectedOptOutDependencyException = new OptOutDependencyException(
                message: "OptOut dependency error occurred, please contact support.",
                innerException: failedOptOutStorageException);

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
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedOptOutServiceException = new FailedOptOutServiceException(
                message: "Failed optOut service error occurred, please contact support.",
                innerException: serviceException);

            var expectedOptOutServiceException = new OptOutServiceException(
                message: "OptOut service error occurred, please contact support.",
                innerException: failedOptOutServiceException);

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