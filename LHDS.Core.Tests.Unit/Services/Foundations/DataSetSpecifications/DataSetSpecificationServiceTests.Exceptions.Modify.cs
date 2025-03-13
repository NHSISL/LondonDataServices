// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            SqlException sqlException = GetSqlException();

            var failedDataSetSpecificationStorageException =
                new FailedDataSetSpecificationStorageException(
                    message: "Failed dataSetSpecification storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDataSetSpecificationDependencyException =
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification dependency error occurred, please contact support.",
                    innerException: failedDataSetSpecificationStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DataSetSpecification> modifyDataSetSpecificationTask =
                this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(randomDataSetSpecification);

            DataSetSpecificationDependencyException actualDataSetSpecificationDependencyException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyException>(
                    modifyDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationDependencyException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(randomDataSetSpecification.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetSpecificationAsync(randomDataSetSpecification),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            DataSetSpecification someDataSetSpecification = CreateRandomDataSetSpecification();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidDataSetSpecificationReferenceException =
                new InvalidDataSetSpecificationReferenceException(
                    message: "Invalid dataSetSpecification reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedDataSetSpecificationDependencyValidationException =
                new DataSetSpecificationDependencyValidationException(
                    message: "DataSetSpecification dependency validation occurred, please try again.",
                    innerException: invalidDataSetSpecificationReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<DataSetSpecification> modifyDataSetSpecificationTask =
                this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(someDataSetSpecification);

            DataSetSpecificationDependencyValidationException actualDataSetSpecificationDependencyValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyValidationException>(
                    modifyDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(someDataSetSpecification.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedDataSetSpecificationDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetSpecificationAsync(someDataSetSpecification),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            var databaseUpdateException = new DbUpdateException();

            var failedDataSetSpecificationStorageException =
                new FailedDataSetSpecificationStorageException(
                    message: "Failed dataSetSpecification storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedDataSetSpecificationDependencyException =
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification dependency error occurred, please contact support.",
                    innerException: failedDataSetSpecificationStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<DataSetSpecification> modifyDataSetSpecificationTask =
                this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(randomDataSetSpecification);

            DataSetSpecificationDependencyException actualDataSetSpecificationDependencyException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyException>(
                    modifyDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationDependencyException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(randomDataSetSpecification.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetSpecificationAsync(randomDataSetSpecification),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedDataSetSpecificationException =
                new LockedDataSetSpecificationException(
                    message: "Locked dataSetSpecification record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedDataSetSpecificationDependencyValidationException =
                new DataSetSpecificationDependencyValidationException(
                    message: "DataSetSpecification dependency validation occurred, please try again.",
                    innerException: lockedDataSetSpecificationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<DataSetSpecification> modifyDataSetSpecificationTask =
                this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(randomDataSetSpecification);

            DataSetSpecificationDependencyValidationException actualDataSetSpecificationDependencyValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyValidationException>(
                    modifyDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(randomDataSetSpecification.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetSpecificationAsync(randomDataSetSpecification),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            var serviceException = new Exception();

            var failedDataSetSpecificationServiceException =
                new FailedDataSetSpecificationServiceException(
                    message: "Failed dataSetSpecification service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetSpecificationServiceException =
                new DataSetSpecificationServiceException(
                    message: "DataSetSpecification service error occurred, please contact support.",
                    innerException: failedDataSetSpecificationServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DataSetSpecification> modifyDataSetSpecificationTask =
                this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(randomDataSetSpecification);

            DataSetSpecificationServiceException actualDataSetSpecificationServiceException =
                await Assert.ThrowsAsync<DataSetSpecificationServiceException>(
                    modifyDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationServiceException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(randomDataSetSpecification.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetSpecificationAsync(randomDataSetSpecification),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}