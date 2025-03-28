// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            SqlException sqlException = GetSqlException();

            var failedSpecificationObjectStorageException =
                new FailedSpecificationObjectStorageException(
                    message: "Failed specificationObject storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSpecificationObjectDependencyException =
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject dependency error occurred, please contact support.",
                    innerException: failedSpecificationObjectStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(randomSpecificationObject.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                this.specificationObjectService.RemoveSpecificationObjectByIdAsync(randomSpecificationObject.Id);

            SpecificationObjectDependencyException actualSpecificationObjectDependencyException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyException>(
                    addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectDependencyException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(randomSpecificationObject.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
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
            Guid someSpecificationObjectId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedSpecificationObjectException =
                new LockedSpecificationObjectException(
                    message: "Locked specificationObject record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedSpecificationObjectDependencyValidationException =
                new SpecificationObjectDependencyValidationException(
                    message: "SpecificationObject dependency validation occurred, please try again.",
                    innerException: lockedSpecificationObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<SpecificationObject> removeSpecificationObjectByIdTask =
                this.specificationObjectService.RemoveSpecificationObjectByIdAsync(someSpecificationObjectId);

            SpecificationObjectDependencyValidationException actualSpecificationObjectDependencyValidationException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyValidationException>(
                    removeSpecificationObjectByIdTask.AsTask);

            // then
            actualSpecificationObjectDependencyValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someSpecificationObjectId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedSpecificationObjectStorageException =
                new FailedSpecificationObjectStorageException(
                    message: "Failed specificationObject storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSpecificationObjectDependencyException =
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject dependency error occurred, please contact support.",
                    innerException: failedSpecificationObjectStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SpecificationObject> deleteSpecificationObjectTask =
                this.specificationObjectService.RemoveSpecificationObjectByIdAsync(someSpecificationObjectId);

            SpecificationObjectDependencyException actualSpecificationObjectDependencyException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyException>(
                    deleteSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectDependencyException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someSpecificationObjectId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedSpecificationObjectServiceException =
                new FailedSpecificationObjectServiceException(
                    message: "Failed specificationObject service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSpecificationObjectServiceException =
                new SpecificationObjectServiceException(
                    message: "SpecificationObject service error occurred, please contact support.",
                    innerException: failedSpecificationObjectServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SpecificationObject> removeSpecificationObjectByIdTask =
                this.specificationObjectService.RemoveSpecificationObjectByIdAsync(someSpecificationObjectId);

            SpecificationObjectServiceException actualSpecificationObjectServiceException =
                await Assert.ThrowsAsync<SpecificationObjectServiceException>(
                    removeSpecificationObjectByIdTask.AsTask);

            // then
            actualSpecificationObjectServiceException.Should()
                .BeEquivalentTo(expectedSpecificationObjectServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}