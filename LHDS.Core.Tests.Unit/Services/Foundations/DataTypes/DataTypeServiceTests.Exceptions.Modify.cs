// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataTypes
{
    public partial class DataTypeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DataType randomDataType = CreateRandomDataType();
            SqlException sqlException = GetSqlException();

            var failedDataTypeStorageException =
                new FailedDataTypeStorageException(
                    message: "Failed dataType storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDataTypeDependencyException =
                new DataTypeDependencyException(
                    message: "DataType dependency error occurred, please contact support.",
                    innerException: failedDataTypeStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(randomDataType);

            DataTypeDependencyException actualDataTypeDependencyException =
                await Assert.ThrowsAsync<DataTypeDependencyException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeDependencyException.Should()
                .BeEquivalentTo(expectedDataTypeDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(randomDataType.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDataTypeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(randomDataType),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            DataType someDataType = CreateRandomDataType();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidDataTypeReferenceException =
                new InvalidDataTypeReferenceException(
                    message: "Invalid dataType reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            DataTypeDependencyValidationException expectedDataTypeDependencyValidationException =
                new DataTypeDependencyValidationException(
                    message: "DataType dependency validation occurred, please try again.",
                    innerException: invalidDataTypeReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(someDataType);

            DataTypeDependencyValidationException actualDataTypeDependencyValidationException =
                await Assert.ThrowsAsync<DataTypeDependencyValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataTypeDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(someDataType.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedDataTypeDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(someDataType),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            DataType randomDataType = CreateRandomDataType();
            var databaseUpdateException = new DbUpdateException();

            var failedDataTypeStorageException =
                new FailedDataTypeStorageException(
                    message: "Failed dataType storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedDataTypeDependencyException =
                new DataTypeDependencyException(
                    message: "DataType dependency error occurred, please contact support.",
                    innerException: failedDataTypeStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(randomDataType);

            DataTypeDependencyException actualDataTypeDependencyException =
                await Assert.ThrowsAsync<DataTypeDependencyException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeDependencyException.Should()
                .BeEquivalentTo(expectedDataTypeDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(randomDataType.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(randomDataType),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            DataType randomDataType = CreateRandomDataType();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedDataTypeException =
                new LockedDataTypeException(
                    message: "Locked dataType record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedDataTypeDependencyValidationException =
                new DataTypeDependencyValidationException(
                    message: "DataType dependency validation occurred, please try again.",
                    innerException: lockedDataTypeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(randomDataType);

            DataTypeDependencyValidationException actualDataTypeDependencyValidationException =
                await Assert.ThrowsAsync<DataTypeDependencyValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataTypeDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(randomDataType.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(randomDataType),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            DataType randomDataType = CreateRandomDataType();
            var serviceException = new Exception();

            var failedDataTypeServiceException =
                new FailedDataTypeServiceException(
                    message: "Failed dataType service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataTypeServiceException =
                new DataTypeServiceException(
                    message: "DataType service error occurred, please contact support.",
                    innerException: failedDataTypeServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(randomDataType);

            DataTypeServiceException actualDataTypeServiceException =
                await Assert.ThrowsAsync<DataTypeServiceException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeServiceException.Should()
                .BeEquivalentTo(expectedDataTypeServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(randomDataType.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(randomDataType),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}