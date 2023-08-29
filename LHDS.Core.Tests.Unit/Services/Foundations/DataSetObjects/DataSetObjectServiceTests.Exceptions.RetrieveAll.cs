using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.DataSetObjects.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedDataSetObjectStorageException =
                new FailedDataSetObjectStorageException(
                    message: "Failed dataSetObject storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedDataSetObjectDependencyException =
                new DataSetObjectDependencyException(
                    message: "DataSetObject dependency error occurred, contact support.",
                    innerException: failedDataSetObjectStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSetObjects())
                    .Throws(sqlException);

            // when
            Action retrieveAllDataSetObjectsAction = () =>
                this.dataSetObjectService.RetrieveAllDataSetObjects();

            DataSetObjectDependencyException actualDataSetObjectDependencyException =
                Assert.Throws<DataSetObjectDependencyException>(retrieveAllDataSetObjectsAction);

            // then
            actualDataSetObjectDependencyException.Should()
                .BeEquivalentTo(expectedDataSetObjectDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataSetObjects(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataSetObjectDependencyException))),
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

            var failedDataSetObjectServiceException =
                new FailedDataSetObjectServiceException(
                    message: "Failed dataSetObject service occurred, please contact support", 
                    innerException: serviceException);

            var expectedDataSetObjectServiceException =
                new DataSetObjectServiceException(
                    message: "DataSetObject service error occurred, contact support.",
                    innerException: failedDataSetObjectServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSetObjects())
                    .Throws(serviceException);

            // when
            Action retrieveAllDataSetObjectsAction = () =>
                this.dataSetObjectService.RetrieveAllDataSetObjects();

            DataSetObjectServiceException actualDataSetObjectServiceException =
                Assert.Throws<DataSetObjectServiceException>(retrieveAllDataSetObjectsAction);

            // then
            actualDataSetObjectServiceException.Should()
                .BeEquivalentTo(expectedDataSetObjectServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataSetObjects(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}