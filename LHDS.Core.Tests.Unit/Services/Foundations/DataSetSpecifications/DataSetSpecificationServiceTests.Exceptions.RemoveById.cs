// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            DataSetSpecification randomDataSetSpecification =
                CreateRandomDataSetSpecification(randomDateTimeOffset, randomEntraUserId);

            SqlException sqlException = GetSqlException();

            var failedDataSetSpecificationStorageException =
                new FailedDataSetSpecificationStorageException(
                    message: "Failed dataSetSpecification storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDataSetSpecificationDependencyException =
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification dependency error occurred, please contact support.",
                    innerException: failedDataSetSpecificationStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetSpecificationByIdAsync(randomDataSetSpecification.Id))
                    .ThrowsAsync(sqlException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);

            DataSetSpecificationDependencyException actualDataSetSpecificationDependencyException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyException>(
                    addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationDependencyException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(randomDataSetSpecification.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someDataSetSpecificationId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedDataSetSpecificationException =
                new LockedDataSetSpecificationException(
                    message: "Locked dataSetSpecification record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedDataSetSpecificationDependencyValidationException =
                new DataSetSpecificationDependencyValidationException(
                    message: "DataSetSpecification dependency validation occurred, please try again.",
                    innerException: lockedDataSetSpecificationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetSpecificationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<DataSetSpecification> removeDataSetSpecificationByIdTask =
                this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(someDataSetSpecificationId);

            DataSetSpecificationDependencyValidationException actualDataSetSpecificationDependencyValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyValidationException>(
                    removeDataSetSpecificationByIdTask.AsTask);

            // then
            actualDataSetSpecificationDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someDataSetSpecificationId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedDataSetSpecificationStorageException =
                new FailedDataSetSpecificationStorageException(
                    message: "Failed dataSetSpecification storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDataSetSpecificationDependencyException =
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification dependency error occurred, please contact support.",
                    innerException: failedDataSetSpecificationStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetSpecificationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DataSetSpecification> deleteDataSetSpecificationTask =
                this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(someDataSetSpecificationId);

            DataSetSpecificationDependencyException actualDataSetSpecificationDependencyException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyException>(
                    deleteDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationDependencyException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someDataSetSpecificationId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedDataSetSpecificationServiceException =
                new FailedDataSetSpecificationServiceException(
                    message: "Failed dataSetSpecification service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetSpecificationServiceException =
                new DataSetSpecificationServiceException(
                    message: "DataSetSpecification service error occurred, please contact support.",
                    innerException: failedDataSetSpecificationServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetSpecificationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DataSetSpecification> removeDataSetSpecificationByIdTask =
                this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(someDataSetSpecificationId);

            DataSetSpecificationServiceException actualDataSetSpecificationServiceException =
                await Assert.ThrowsAsync<DataSetSpecificationServiceException>(
                    removeDataSetSpecificationByIdTask.AsTask);

            // then
            actualDataSetSpecificationServiceException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}