using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject someDataSetObject = CreateRandomDataSetObject();
            SqlException sqlException = GetSqlException();

            var failedDataSetObjectStorageException =
                new FailedDataSetObjectStorageException(
                    message: "Failed dataSetObject storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedDataSetObjectDependencyException =
                new DataSetObjectDependencyException(
                    message: "DataSetObject dependency error occurred, contact support.",
                    innerException: failedDataSetObjectStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<SpecificationObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(someDataSetObject);

            DataSetObjectDependencyException actualDataSetObjectDependencyException =
                await Assert.ThrowsAsync<DataSetObjectDependencyException>(
                    addDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectDependencyException.Should()
                .BeEquivalentTo(expectedDataSetObjectDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataSetObjectDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDataSetObjectAlreadyExsitsAndLogItAsync()
        {
            // given
            SpecificationObject randomDataSetObject = CreateRandomDataSetObject();
            SpecificationObject alreadyExistsDataSetObject = randomDataSetObject;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsDataSetObjectException =
                new AlreadyExistsDataSetObjectException(
                    message: "DataSetObject with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedDataSetObjectDependencyValidationException =
                new DataSetObjectDependencyValidationException(
                    message: "DataSetObject dependency validation occurred, please try again.",
                    innerException: alreadyExistsDataSetObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<SpecificationObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(alreadyExistsDataSetObject);

            // then
            DataSetObjectDependencyValidationException actualDataSetObjectDependencyValidationException =
                await Assert.ThrowsAsync<DataSetObjectDependencyValidationException>(
                    addDataSetObjectTask.AsTask);

            actualDataSetObjectDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject someDataSetObject = CreateRandomDataSetObject();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidDataSetObjectReferenceException =
                new InvalidDataSetObjectReferenceException(
                    message: "Invalid dataSetObject reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedDataSetObjectValidationException =
                new DataSetObjectDependencyValidationException(
                    message: "DataSetObject dependency validation occurred, please try again.",
                    innerException: invalidDataSetObjectReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<SpecificationObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(someDataSetObject);

            // then
            DataSetObjectDependencyValidationException actualDataSetObjectDependencyValidationException =
                await Assert.ThrowsAsync<DataSetObjectDependencyValidationException>(
                    addDataSetObjectTask.AsTask);

            actualDataSetObjectDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(someDataSetObject),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject someDataSetObject = CreateRandomDataSetObject();

            var databaseUpdateException =
                new DbUpdateException();

            var failedDataSetObjectStorageException =
                new FailedDataSetObjectStorageException(
                    message: "Failed dataSetObject storage error occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedDataSetObjectDependencyException =
                new DataSetObjectDependencyException(
                    message: "DataSetObject dependency error occurred, contact support.",
                    innerException: failedDataSetObjectStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<SpecificationObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(someDataSetObject);

            DataSetObjectDependencyException actualDataSetObjectDependencyException =
                await Assert.ThrowsAsync<DataSetObjectDependencyException>(
                    addDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectDependencyException.Should()
                .BeEquivalentTo(expectedDataSetObjectDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject someDataSetObject = CreateRandomDataSetObject();
            var serviceException = new Exception();

            var failedDataSetObjectServiceException =
                new FailedDataSetObjectServiceException(
                    message: "Failed dataSetObject service occurred, please contact support", 
                    innerException: serviceException);

            var expectedDataSetObjectServiceException =
                new DataSetObjectServiceException(
                    message: "DataSetObject service error occurred, contact support.",
                    innerException: failedDataSetObjectServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<SpecificationObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(someDataSetObject);

            DataSetObjectServiceException actualDataSetObjectServiceException =
                await Assert.ThrowsAsync<DataSetObjectServiceException>(
                    addDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectServiceException.Should()
                .BeEquivalentTo(expectedDataSetObjectServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}