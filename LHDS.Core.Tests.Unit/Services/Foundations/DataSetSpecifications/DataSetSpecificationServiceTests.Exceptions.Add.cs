// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomAuditUserId = GetRandomStringWithLengthOf(255);

            DataSetSpecification someDataSetSpecification = 
                CreateRandomDataSetSpecification(
                    randomDateTimeOffset, 
                    randomAuditUserId);

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
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(someDataSetSpecification);

            DataSetSpecificationDependencyException actualDataSetSpecificationDependencyException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyException>(
                    addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationDependencyException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDataSetSpecificationAlreadyExsitsAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(255);
            EntraUser randomEntraUser = CreateRandomEntraUser(randomUserId);

            DataSetSpecification randomDataSetSpecification =
                CreateRandomDataSetSpecification(randomDateTimeOffset, randomUserId);

            DataSetSpecification alreadyExistsDataSetSpecification = randomDataSetSpecification;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsDataSetSpecificationException =
                new AlreadyExistsDataSetSpecificationException(
                    message: "DataSetSpecification with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedDataSetSpecificationDependencyValidationException =
                new DataSetSpecificationDependencyValidationException(
                    message: "DataSetSpecification dependency validation occurred, please try again.",
                    innerException: alreadyExistsDataSetSpecificationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(duplicateKeyException);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(alreadyExistsDataSetSpecification);

            // then
            DataSetSpecificationDependencyValidationException actualDataSetSpecificationDependencyValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyValidationException>(
                    addDataSetSpecificationTask.AsTask);

            actualDataSetSpecificationDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(255);
            EntraUser randomEntraUser = CreateRandomEntraUser(randomUserId);

            DataSetSpecification randomDataSetSpecification =
                CreateRandomDataSetSpecification(randomDateTimeOffset, randomUserId);

            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidDataSetSpecificationReferenceException =
                new InvalidDataSetSpecificationReferenceException(
                    message: "Invalid dataSetSpecification reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationDependencyValidationException(
                    message: "DataSetSpecification dependency validation occurred, please try again.",
                    innerException: invalidDataSetSpecificationReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(randomDataSetSpecification);

            // then
            DataSetSpecificationDependencyValidationException actualDataSetSpecificationDependencyValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyValidationException>(
                    addDataSetSpecificationTask.AsTask);

            actualDataSetSpecificationDependencyValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(randomDataSetSpecification),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(255);
            EntraUser randomEntraUser = CreateRandomEntraUser(randomUserId);

            DataSetSpecification randomDataSetSpecification =
                CreateRandomDataSetSpecification(randomDateTimeOffset, randomUserId);

            var databaseUpdateException =
                new DbUpdateException();

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

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(randomDataSetSpecification);

            DataSetSpecificationDependencyException actualDataSetSpecificationDependencyException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyException>(
                    addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationDependencyException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(255);
            EntraUser randomEntraUser = CreateRandomEntraUser(randomUserId);

            DataSetSpecification randomDataSetSpecification =
                CreateRandomDataSetSpecification(randomDateTimeOffset, randomUserId);

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

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(randomDataSetSpecification);

            DataSetSpecificationServiceException actualDataSetSpecificationServiceException =
                await Assert.ThrowsAsync<DataSetSpecificationServiceException>(
                    addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationServiceException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}