// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id);

            TerminologyArtifactDependencyException actualTerminologyArtifactDependencyException =
                await Assert.ThrowsAsync<TerminologyArtifactDependencyException>(
                    addTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
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
            Guid someTerminologyArtifactId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedTerminologyArtifactException =
                new LockedTerminologyArtifactException(
                    message: "Locked terminologyArtifact record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedTerminologyArtifactDependencyValidationException =
                new TerminologyArtifactDependencyValidationException(
                    message: "TerminologyArtifact dependency validation occurred, please try again.",
                    innerException: lockedTerminologyArtifactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<TerminologyArtifact> removeTerminologyArtifactByIdTask =
                this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(someTerminologyArtifactId);

            TerminologyArtifactDependencyValidationException actualTerminologyArtifactDependencyValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactDependencyValidationException>(
                    removeTerminologyArtifactByIdTask.AsTask);

            // then
            actualTerminologyArtifactDependencyValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTerminologyArtifactId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedTerminologyArtifactStorageException =
                new FailedTerminologyArtifactStorageException(
                    message: "Failed terminologyArtifact storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedTerminologyArtifactDependencyException =
                new TerminologyArtifactDependencyException(
                    message: "TerminologyArtifact dependency error occurred, please contact support.",
                    innerException: failedTerminologyArtifactStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TerminologyArtifact> deleteTerminologyArtifactTask =
                this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(someTerminologyArtifactId);

            TerminologyArtifactDependencyException actualTerminologyArtifactDependencyException =
                await Assert.ThrowsAsync<TerminologyArtifactDependencyException>(
                    deleteTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someTerminologyArtifactId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedTerminologyArtifactServiceException =
                new FailedTerminologyArtifactServiceException(
                    message: "Failed terminologyArtifact service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedTerminologyArtifactServiceException =
                new TerminologyArtifactServiceException(
                    message: "TerminologyArtifact service error occurred, please contact support.",
                    innerException: failedTerminologyArtifactServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<TerminologyArtifact> removeTerminologyArtifactByIdTask =
                this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(someTerminologyArtifactId);

            TerminologyArtifactServiceException actualTerminologyArtifactServiceException =
                await Assert.ThrowsAsync<TerminologyArtifactServiceException>(
                    removeTerminologyArtifactByIdTask.AsTask);

            // then
            actualTerminologyArtifactServiceException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}