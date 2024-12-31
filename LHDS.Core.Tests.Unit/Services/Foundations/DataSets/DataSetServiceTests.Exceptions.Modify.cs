// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(randomDataSet);

            DataSetDependencyException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetDependencyException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetDependencyException.Should()
                .BeEquivalentTo(expectedDataSetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(randomDataSet.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetAsync(randomDataSet),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            DataSet someDataSet = CreateRandomDataSet();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidDataSetReferenceException =
                new InvalidDataSetReferenceException(
                    message: "Invalid dataSet reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            DataSetDependencyValidationException expectedDataSetDependencyValidationException =
                new DataSetDependencyValidationException(
                    message: "DataSet dependency validation occurred, please try again.",
                    innerException: invalidDataSetReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(someDataSet);

            DataSetDependencyValidationException actualDataSetDependencyValidationException =
                await Assert.ThrowsAsync<DataSetDependencyValidationException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(someDataSet.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedDataSetDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetAsync(someDataSet),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            DataSet randomDataSet = CreateRandomDataSet();
            var databaseUpdateException = new DbUpdateException();

            var failedDataSetStorageException =
                new FailedDataSetStorageException(
                    message: "Failed dataSet storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedDataSetDependencyException =
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, please contact support.",
                    innerException: failedDataSetStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(randomDataSet);

            DataSetDependencyException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetDependencyException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetDependencyException.Should()
                .BeEquivalentTo(expectedDataSetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(randomDataSet.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetAsync(randomDataSet),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            DataSet randomDataSet = CreateRandomDataSet();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedDataSetException =
                new LockedDataSetException(
                    message: "Locked dataSet record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedDataSetDependencyValidationException =
                new DataSetDependencyValidationException(
                    message: "DataSet dependency validation occurred, please try again.",
                    innerException: lockedDataSetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(randomDataSet);

            DataSetDependencyValidationException actualDataSetDependencyValidationException =
                await Assert.ThrowsAsync<DataSetDependencyValidationException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(randomDataSet.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetAsync(randomDataSet),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            DataSet randomDataSet = CreateRandomDataSet();
            var serviceException = new Exception();

            var failedDataSetServiceException =
                new FailedDataSetServiceException(
                    message: "Failed dataSet service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetServiceException =
                new DataSetServiceException(
                    message: "DataSet service error occurred, please contact support.",
                    innerException: failedDataSetServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(randomDataSet);

            DataSetServiceException actualDataSetServiceException =
                await Assert.ThrowsAsync<DataSetServiceException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetServiceException.Should()
                .BeEquivalentTo(expectedDataSetServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(randomDataSet.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetAsync(randomDataSet),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}