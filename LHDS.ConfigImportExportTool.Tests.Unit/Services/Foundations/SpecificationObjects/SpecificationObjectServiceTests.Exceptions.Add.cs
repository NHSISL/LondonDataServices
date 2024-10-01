// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject someSpecificationObject = CreateRandomSpecificationObject();
            SqlException sqlException = GetSqlException();

            var failedSpecificationObjectStorageException =
                new FailedSpecificationObjectStorageException(
                    message: "Failed specificationObject storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSpecificationObjectDependencyException =
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject dependency error occurred, please contact support.",
                    innerException: failedSpecificationObjectStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                this.specificationObjectService.AddSpecificationObjectAsync(someSpecificationObject);

            SpecificationObjectDependencyException actualSpecificationObjectDependencyException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyException>(
                    addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectDependencyException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSpecificationObjectDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfSpecificationObjectAlreadyExsitsAndLogItAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            SpecificationObject alreadyExistsSpecificationObject = randomSpecificationObject;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsSpecificationObjectException =
                new AlreadyExistsSpecificationObjectException(
                    message: "SpecificationObject with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedSpecificationObjectDependencyValidationException =
                new SpecificationObjectDependencyValidationException(
                    message: "SpecificationObject dependency validation occurred, please try again.",
                    innerException: alreadyExistsSpecificationObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                this.specificationObjectService.AddSpecificationObjectAsync(alreadyExistsSpecificationObject);

            // then
            SpecificationObjectDependencyValidationException actualSpecificationObjectDependencyValidationException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyValidationException>(
                    addSpecificationObjectTask.AsTask);

            actualSpecificationObjectDependencyValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSpecificationObjectDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject someSpecificationObject = CreateRandomSpecificationObject();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidSpecificationObjectReferenceException =
                new InvalidSpecificationObjectReferenceException(
                    message: "Invalid specificationObject reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectDependencyValidationException(
                    message: "SpecificationObject dependency validation occurred, please try again.",
                    innerException: invalidSpecificationObjectReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                this.specificationObjectService.AddSpecificationObjectAsync(someSpecificationObject);

            // then
            SpecificationObjectDependencyValidationException actualSpecificationObjectDependencyValidationException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyValidationException>(
                    addSpecificationObjectTask.AsTask);

            actualSpecificationObjectDependencyValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(someSpecificationObject),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject someSpecificationObject = CreateRandomSpecificationObject();

            var databaseUpdateException =
                new DbUpdateException();

            var failedSpecificationObjectStorageException =
                new FailedSpecificationObjectStorageException(
                    message: "Failed specificationObject storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedSpecificationObjectDependencyException =
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject dependency error occurred, please contact support.",
                    innerException: failedSpecificationObjectStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                this.specificationObjectService.AddSpecificationObjectAsync(someSpecificationObject);

            SpecificationObjectDependencyException actualSpecificationObjectDependencyException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyException>(
                    addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectDependencyException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSpecificationObjectDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject someSpecificationObject = CreateRandomSpecificationObject();
            var serviceException = new Exception();

            var failedSpecificationObjectServiceException =
                new FailedSpecificationObjectServiceException(
                    message: "Failed specificationObject service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSpecificationObjectServiceException =
                new SpecificationObjectServiceException(
                    message: "SpecificationObject service error occurred, please contact support.",
                    innerException: failedSpecificationObjectServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                this.specificationObjectService.AddSpecificationObjectAsync(someSpecificationObject);

            SpecificationObjectServiceException actualSpecificationObjectServiceException =
                await Assert.ThrowsAsync<SpecificationObjectServiceException>(
                    addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectServiceException.Should()
                .BeEquivalentTo(expectedSpecificationObjectServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSpecificationObjectServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}