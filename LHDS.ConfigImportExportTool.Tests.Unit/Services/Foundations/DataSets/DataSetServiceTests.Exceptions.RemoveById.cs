// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
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
                    message: "Failed dataSet storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDataSetDependencyException =
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, please contact support.",
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
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDataSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
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
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someDataSetId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedDataSetStorageException =
                new FailedDataSetStorageException(
                    message: "Failed dataSet storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDataSetDependencyException =
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, please contact support.",
                    innerException: failedDataSetStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DataSet> deleteDataSetTask =
                this.dataSetService.RemoveDataSetByIdAsync(someDataSetId);

            DataSetDependencyException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetDependencyException>(
                    deleteDataSetTask.AsTask);

            // then
            actualDataSetDependencyException.Should()
                .BeEquivalentTo(expectedDataSetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDataSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someDataSetId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedDataSetServiceException =
                new FailedDataSetServiceException(
                    message: "Failed dataSet service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetServiceException =
                new DataSetServiceException(
                    message: "DataSet service error occurred, please contact support.",
                    innerException: failedDataSetServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DataSet> removeDataSetByIdTask =
                this.dataSetService.RemoveDataSetByIdAsync(someDataSetId);

            DataSetServiceException actualDataSetServiceException =
                await Assert.ThrowsAsync<DataSetServiceException>(
                    removeDataSetByIdTask.AsTask);

            // then
            actualDataSetServiceException.Should()
                .BeEquivalentTo(expectedDataSetServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}