using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Foundations.DataSetObjects;
using LHDS.Core.Models.Foundations.DataSetObjects.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DataSetObject randomDataSetObject = CreateRandomDataSetObject();
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
            ValueTask<DataSetObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(randomDataSetObject);

            DataSetObjectDependencyException actualDataSetObjectDependencyException =
                await Assert.ThrowsAsync<DataSetObjectDependencyException>(
                    modifyDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectDependencyException.Should()
                .BeEquivalentTo(expectedDataSetObjectDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(randomDataSetObject.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataSetObjectDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetObjectAsync(randomDataSetObject),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            DataSetObject someDataSetObject = CreateRandomDataSetObject();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidDataSetObjectReferenceException =
                new InvalidDataSetObjectReferenceException(
                    message: "Invalid dataSetObject reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            DataSetObjectDependencyValidationException expectedDataSetObjectDependencyValidationException =
                new DataSetObjectDependencyValidationException(
                    message: "DataSetObject dependency validation occurred, please try again.",
                    innerException: invalidDataSetObjectReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<DataSetObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(someDataSetObject);

            DataSetObjectDependencyValidationException actualDataSetObjectDependencyValidationException =
                await Assert.ThrowsAsync<DataSetObjectDependencyValidationException>(
                    modifyDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(someDataSetObject.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedDataSetObjectDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetObjectAsync(someDataSetObject),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            DataSetObject randomDataSetObject = CreateRandomDataSetObject();
            var databaseUpdateException = new DbUpdateException();

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
            ValueTask<DataSetObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(randomDataSetObject);

            DataSetObjectDependencyException actualDataSetObjectDependencyException =
                await Assert.ThrowsAsync<DataSetObjectDependencyException>(
                    modifyDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectDependencyException.Should()
                .BeEquivalentTo(expectedDataSetObjectDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(randomDataSetObject.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetObjectAsync(randomDataSetObject),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}