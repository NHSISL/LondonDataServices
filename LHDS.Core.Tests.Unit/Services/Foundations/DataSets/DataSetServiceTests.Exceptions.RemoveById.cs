using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DataSet randomDataSet = CreateRandomDataSet();
            SqlException sqlException = GetSqlException();

            var failedDataSetStorageException =
                new FailedDataSetStorageException(
                    message: "Failed dataSet storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedDataSetDependencyException =
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, contact support.",
                    innerException: failedDataSetStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(randomDataSet.Id))
                    .Throws(sqlException);

            // when
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.RemoveDataSetByIdAsync(randomDataSet.Id);

            DataSetDependencyException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetDependencyException>(
                    addDataSetTask.AsTask);

            // then
            actualDataSetDependencyException.Should()
                .BeEquivalentTo(expectedDataSetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(randomDataSet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetAsync(It.IsAny<DataSet>()),
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
            Guid someDataSetId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedDataSetException =
                new LockedDataSetException(
                    message: "Locked dataSet record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedDataSetDependencyValidationException =
                new DataSetDependencyValidationException(
                    message: "DataSet dependency validation occurred, please try again.",
                    innerException: lockedDataSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<DataSet> removeDataSetByIdTask =
                this.dataSetService.RemoveDataSetByIdAsync(someDataSetId);

            DataSetDependencyValidationException actualDataSetDependencyValidationException =
                await Assert.ThrowsAsync<DataSetDependencyValidationException>(
                    removeDataSetByIdTask.AsTask);

            // then
            actualDataSetDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}