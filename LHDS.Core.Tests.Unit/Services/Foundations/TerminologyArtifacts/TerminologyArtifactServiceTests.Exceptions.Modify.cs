// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            SqlException sqlException = GetSqlException();

            var failedTerminologyArtifactStorageException =
                new FailedTerminologyArtifactStorageException(
                    message: "Failed terminologyArtifact storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedTerminologyArtifactDependencyException =
                new TerminologyArtifactDependencyException(
                    message: "TerminologyArtifact dependency error occurred, please contact support.",
                    innerException: failedTerminologyArtifactStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Throws(sqlException);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(randomTerminologyArtifact);

            TerminologyArtifactDependencyException actualTerminologyArtifactDependencyException =
                await Assert.ThrowsAsync<TerminologyArtifactDependencyException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(randomTerminologyArtifact),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            TerminologyArtifact someTerminologyArtifact = CreateRandomTerminologyArtifact();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidTerminologyArtifactReferenceException =
                new InvalidTerminologyArtifactReferenceException(
                    message: "Invalid terminologyArtifact reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            TerminologyArtifactDependencyValidationException expectedTerminologyArtifactDependencyValidationException =
                new TerminologyArtifactDependencyValidationException(
                    message: "TerminologyArtifact dependency validation occurred, please try again.",
                    innerException: invalidTerminologyArtifactReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(someTerminologyArtifact);

            TerminologyArtifactDependencyValidationException actualTerminologyArtifactDependencyValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactDependencyValidationException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactDependencyValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(someTerminologyArtifact.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTerminologyArtifactDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(someTerminologyArtifact),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            var databaseUpdateException = new DbUpdateException();

            var failedTerminologyArtifactStorageException =
                new FailedTerminologyArtifactStorageException(
                    message: "Failed terminologyArtifact storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedTerminologyArtifactDependencyException =
                new TerminologyArtifactDependencyException(
                    message: "TerminologyArtifact dependency error occurred, please contact support.",
                    innerException: failedTerminologyArtifactStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(randomTerminologyArtifact);

            TerminologyArtifactDependencyException actualTerminologyArtifactDependencyException =
                await Assert.ThrowsAsync<TerminologyArtifactDependencyException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(randomTerminologyArtifact),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTerminologyArtifactException =
                new LockedTerminologyArtifactException(
                    message: "Locked terminologyArtifact record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedTerminologyArtifactDependencyValidationException =
                new TerminologyArtifactDependencyValidationException(
                    message: "TerminologyArtifact dependency validation occurred, please try again.",
                    innerException: lockedTerminologyArtifactException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(randomTerminologyArtifact);

            TerminologyArtifactDependencyValidationException actualTerminologyArtifactDependencyValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactDependencyValidationException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactDependencyValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(randomTerminologyArtifact),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            var serviceException = new Exception();

            var failedTerminologyArtifactServiceException =
                new FailedTerminologyArtifactServiceException(
                    message: "Failed terminologyArtifact service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedTerminologyArtifactServiceException =
                new TerminologyArtifactServiceException(
                    message: "TerminologyArtifact service error occurred, please contact support.",
                    innerException: failedTerminologyArtifactServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Throws(serviceException);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(randomTerminologyArtifact);

            TerminologyArtifactServiceException actualTerminologyArtifactServiceException =
                await Assert.ThrowsAsync<TerminologyArtifactServiceException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactServiceException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(randomTerminologyArtifact),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}