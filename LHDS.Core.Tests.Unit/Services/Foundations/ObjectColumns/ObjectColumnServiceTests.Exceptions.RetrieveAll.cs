// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedObjectColumnStorageException =
                new FailedObjectColumnStorageException(
                    message: "Failed objectColumn storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedObjectColumnDependencyException =
                new ObjectColumnDependencyException(
                    message: "ObjectColumn dependency error occurred, please contact support.",
                    innerException: failedObjectColumnStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllObjectColumns())
                    .Throws(sqlException);

            // when
            Action retrieveAllObjectColumnsAction = () =>
                this.objectColumnService.RetrieveAllObjectColumns();

            ObjectColumnDependencyException actualObjectColumnDependencyException =
                Assert.Throws<ObjectColumnDependencyException>(retrieveAllObjectColumnsAction);

            // then
            actualObjectColumnDependencyException.Should()
                .BeEquivalentTo(expectedObjectColumnDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllObjectColumns(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedObjectColumnDependencyException))),
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

            var failedObjectColumnServiceException =
                new FailedObjectColumnServiceException(
                    message: "Failed objectColumn service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedObjectColumnServiceException =
                new ObjectColumnServiceException(
                    message: "ObjectColumn service error occurred, please contact support.",
                    innerException: failedObjectColumnServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllObjectColumns())
                    .Throws(serviceException);

            // when
            Action retrieveAllObjectColumnsAction = () =>
                this.objectColumnService.RetrieveAllObjectColumns();

            ObjectColumnServiceException actualObjectColumnServiceException =
                Assert.Throws<ObjectColumnServiceException>(retrieveAllObjectColumnsAction);

            // then
            actualObjectColumnServiceException.Should()
                .BeEquivalentTo(expectedObjectColumnServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllObjectColumns(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedObjectColumnServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}