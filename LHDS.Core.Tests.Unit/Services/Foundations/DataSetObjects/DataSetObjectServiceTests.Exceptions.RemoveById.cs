using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject randomDataSetObject = CreateRandomDataSetObject();
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
                broker.SelectDataSetObjectByIdAsync(randomDataSetObject.Id))
                    .Throws(sqlException);

            // when
            ValueTask<SpecificationObject> addDataSetObjectTask =
                this.dataSetObjectService.RemoveDataSetObjectByIdAsync(randomDataSetObject.Id);

            DataSetObjectDependencyException actualDataSetObjectDependencyException =
                await Assert.ThrowsAsync<DataSetObjectDependencyException>(
                    addDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectDependencyException.Should()
                .BeEquivalentTo(expectedDataSetObjectDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(randomDataSetObject.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataSetObjectDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someDataSetObjectId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedDataSetObjectException =
                new LockedDataSetObjectException(
                    message: "Locked dataSetObject record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedDataSetObjectDependencyValidationException =
                new DataSetObjectDependencyValidationException(
                    message: "DataSetObject dependency validation occurred, please try again.",
                    innerException: lockedDataSetObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<SpecificationObject> removeDataSetObjectByIdTask =
                this.dataSetObjectService.RemoveDataSetObjectByIdAsync(someDataSetObjectId);

            DataSetObjectDependencyValidationException actualDataSetObjectDependencyValidationException =
                await Assert.ThrowsAsync<DataSetObjectDependencyValidationException>(
                    removeDataSetObjectByIdTask.AsTask);

            // then
            actualDataSetObjectDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someDataSetObjectId = Guid.NewGuid();
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
            ValueTask<SpecificationObject> deleteDataSetObjectTask =
                this.dataSetObjectService.RemoveDataSetObjectByIdAsync(someDataSetObjectId);

            DataSetObjectDependencyException actualDataSetObjectDependencyException =
                await Assert.ThrowsAsync<DataSetObjectDependencyException>(
                    deleteDataSetObjectTask.AsTask);

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
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someDataSetObjectId = Guid.NewGuid();
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
            ValueTask<SpecificationObject> removeDataSetObjectByIdTask =
                this.dataSetObjectService.RemoveDataSetObjectByIdAsync(someDataSetObjectId);

            DataSetObjectServiceException actualDataSetObjectServiceException =
                await Assert.ThrowsAsync<DataSetObjectServiceException>(
                    removeDataSetObjectByIdTask.AsTask);

            // then
            actualDataSetObjectServiceException.Should()
                .BeEquivalentTo(expectedDataSetObjectServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

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