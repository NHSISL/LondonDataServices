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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DataSet someDataSet = CreateRandomDataSet();
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
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(someDataSet);

            DataSetDependencyException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetDependencyException>(
                    addDataSetTask.AsTask);

            // then
            actualDataSetDependencyException.Should()
                .BeEquivalentTo(expectedDataSetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDataSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDataSetAlreadyExistsAndLogItAsync()
        {
            // given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet alreadyExistsDataSet = randomDataSet;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsDataSetException =
                new AlreadyExistsDataSetException(
                    message: "DataSet with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedDataSetDependencyValidationException =
                new DataSetDependencyValidationException(
                    message: "DataSet dependency validation occurred, please try again.",
                    innerException: alreadyExistsDataSetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(alreadyExistsDataSet);

            // then
            DataSetDependencyValidationException actualDataSetDependencyValidationException =
                await Assert.ThrowsAsync<DataSetDependencyValidationException>(
                    addDataSetTask.AsTask);

            actualDataSetDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
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

            var expectedDataSetValidationException =
                new DataSetDependencyValidationException(
                    message: "DataSet dependency validation occurred, please try again.",
                    innerException: invalidDataSetReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(someDataSet);

            // then
            DataSetDependencyValidationException actualDataSetDependencyValidationException =
                await Assert.ThrowsAsync<DataSetDependencyValidationException>(
                    addDataSetTask.AsTask);

            actualDataSetDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetAsync(someDataSet),
                    Times.Never());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            DataSet someDataSet = CreateRandomDataSet();

            var databaseUpdateException =
                new DbUpdateException();

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
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(someDataSet);

            DataSetDependencyException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetDependencyException>(
                    addDataSetTask.AsTask);

            // then
            actualDataSetDependencyException.Should()
                .BeEquivalentTo(expectedDataSetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            DataSet someDataSet = CreateRandomDataSet();
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
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(someDataSet);

            DataSetServiceException actualDataSetServiceException =
                await Assert.ThrowsAsync<DataSetServiceException>(
                    addDataSetTask.AsTask);

            // then
            actualDataSetServiceException.Should()
                .BeEquivalentTo(expectedDataSetServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}