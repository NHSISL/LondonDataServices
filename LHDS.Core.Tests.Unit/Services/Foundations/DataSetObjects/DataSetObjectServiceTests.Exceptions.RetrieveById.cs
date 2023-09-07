using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
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
                broker.SelectDataSetObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SpecificationObject> retrieveDataSetObjectByIdTask =
                this.dataSetObjectService.RetrieveDataSetObjectByIdAsync(someId);

            DataSetObjectDependencyException actualDataSetObjectDependencyException =
                await Assert.ThrowsAsync<DataSetObjectDependencyException>(
                    retrieveDataSetObjectByIdTask.AsTask);

            // then
            actualDataSetObjectDependencyException.Should()
                .BeEquivalentTo(expectedDataSetObjectDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedDataSetObjectServiceException =
                new FailedDataSetObjectServiceException(
                    message: "Failed dataSetObject service occurred, please contact support", 
                    innerException: serviceException);

            var expectedDataSetObjectServiceException =
                new DataSetObjectServiceException(
                    message: "DataSetObject service error occurred, contact support.",
                    innerException: failedDataSetObjectServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SpecificationObject> retrieveDataSetObjectByIdTask =
                this.dataSetObjectService.RetrieveDataSetObjectByIdAsync(someId);

            DataSetObjectServiceException actualDataSetObjectServiceException =
                await Assert.ThrowsAsync<DataSetObjectServiceException>(
                    retrieveDataSetObjectByIdTask.AsTask);

            // then
            actualDataSetObjectServiceException.Should()
                .BeEquivalentTo(expectedDataSetObjectServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDataSetObjectServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}