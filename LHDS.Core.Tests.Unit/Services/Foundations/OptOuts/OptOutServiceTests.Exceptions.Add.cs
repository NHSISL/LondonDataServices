// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            SqlException sqlException = GetSqlException();

            var failedOptOutStorageException = new FailedOptOutStorageException(
                message: "Failed optOut storage error occurred, please contact support.",
                innerException: sqlException);

            var expectedOptOutDependencyException = new OptOutDependencyException(
                message: "OptOut dependency error occurred, please contact support.",
                innerException: failedOptOutStorageException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<OptOut>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<OptOut> addOptOutTask =
                this.optOutService.AddOptOutAsync(someOptOut);

            OptOutDependencyException actualOptOutDependencyException =
                await Assert.ThrowsAsync<OptOutDependencyException>(
                    addOptOutTask.AsTask);

            // then
            actualOptOutDependencyException.Should()
                .BeEquivalentTo(expectedOptOutDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<OptOut>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedOptOutDependencyException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfOptOutAlreadyExsitsAndLogItAsync()
        {
            // given
            OptOut randomOptOut = CreateRandomOptOut();
            OptOut alreadyExistsOptOut = randomOptOut;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsOptOutException =
                new AlreadyExistsOptOutException(
                    message: "OptOut with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedOptOutDependencyValidationException = new OptOutDependencyValidationException(
                message: "OptOut dependency validation occurred, please try again.",
                innerException: alreadyExistsOptOutException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<OptOut>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<OptOut> addOptOutTask =
                this.optOutService.AddOptOutAsync(alreadyExistsOptOut);

            // then
            OptOutDependencyValidationException actualOptOutDependencyValidationException =
                await Assert.ThrowsAsync<OptOutDependencyValidationException>(
                    addOptOutTask.AsTask);

            actualOptOutDependencyValidationException.Should()
                .BeEquivalentTo(expectedOptOutDependencyValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<OptOut>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutDependencyValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidOptOutReferenceException = new InvalidOptOutReferenceException(
                message: "Invalid optOut reference error occurred.",
                innerException: foreignKeyConstraintConflictException);

            var expectedOptOutValidationException = new OptOutDependencyValidationException(
                message: "OptOut dependency validation occurred, please try again.",
                innerException: invalidOptOutReferenceException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<OptOut>()))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<OptOut> addOptOutTask =
                this.optOutService.AddOptOutAsync(someOptOut);

            // then
            OptOutDependencyValidationException actualOptOutDependencyValidationException =
                await Assert.ThrowsAsync<OptOutDependencyValidationException>(
                    addOptOutTask.AsTask);

            actualOptOutDependencyValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<OptOut>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(someOptOut),
                    Times.Never());

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();

            var databaseUpdateException =
                new DbUpdateException();

            var failedOptOutStorageException = new FailedOptOutStorageException(
                message: "Failed optOut storage error occurred, please contact support.",
                innerException: databaseUpdateException);

            var expectedOptOutDependencyException = new OptOutDependencyException(
                message: "OptOut dependency error occurred, please contact support.",
                innerException: failedOptOutStorageException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<OptOut>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<OptOut> addOptOutTask =
                this.optOutService.AddOptOutAsync(someOptOut);

            OptOutDependencyException actualOptOutDependencyException =
                await Assert.ThrowsAsync<OptOutDependencyException>(
                    addOptOutTask.AsTask);

            // then
            actualOptOutDependencyException.Should()
                .BeEquivalentTo(expectedOptOutDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<OptOut>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutDependencyException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            var serviceException = new Exception();

            var failedOptOutServiceException = new FailedOptOutServiceException(
                message: "Failed optOut service error occurred, please contact support.",
                innerException: serviceException);

            var expectedOptOutServiceException = new OptOutServiceException(
                message: "OptOut service error occurred, please contact support.",
                innerException: failedOptOutServiceException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<OptOut>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<OptOut> addOptOutTask =
                this.optOutService.AddOptOutAsync(someOptOut);

            OptOutServiceException actualOptOutServiceException =
                await Assert.ThrowsAsync<OptOutServiceException>(
                    addOptOutTask.AsTask);

            // then
            actualOptOutServiceException.Should()
                .BeEquivalentTo(expectedOptOutServiceException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<OptOut>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutServiceException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}