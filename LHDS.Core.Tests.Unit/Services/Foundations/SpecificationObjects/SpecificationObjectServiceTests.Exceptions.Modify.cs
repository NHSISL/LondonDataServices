using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            SqlException sqlException = GetSqlException();

            var failedSpecificationObjectStorageException =
                new FailedSpecificationObjectStorageException(
                    message: "Failed dataSetObject storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedSpecificationObjectDependencyException =
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject dependency error occurred, contact support.",
                    innerException: failedSpecificationObjectStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.dataSetObjectService.ModifySpecificationObjectAsync(randomSpecificationObject);

            SpecificationObjectDependencyException actualSpecificationObjectDependencyException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyException>(
                    modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectDependencyException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(randomSpecificationObject.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSpecificationObjectDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(randomSpecificationObject),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject someSpecificationObject = CreateRandomSpecificationObject();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidSpecificationObjectReferenceException =
                new InvalidSpecificationObjectReferenceException(
                    message: "Invalid dataSetObject reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            SpecificationObjectDependencyValidationException expectedSpecificationObjectDependencyValidationException =
                new SpecificationObjectDependencyValidationException(
                    message: "SpecificationObject dependency validation occurred, please try again.",
                    innerException: invalidSpecificationObjectReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.dataSetObjectService.ModifySpecificationObjectAsync(someSpecificationObject);

            SpecificationObjectDependencyValidationException actualSpecificationObjectDependencyValidationException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyValidationException>(
                    modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectDependencyValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(someSpecificationObject.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSpecificationObjectDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(someSpecificationObject),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            var databaseUpdateException = new DbUpdateException();

            var failedSpecificationObjectStorageException =
                new FailedSpecificationObjectStorageException(
                    message: "Failed dataSetObject storage error occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedSpecificationObjectDependencyException =
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject dependency error occurred, contact support.",
                    innerException: failedSpecificationObjectStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.dataSetObjectService.ModifySpecificationObjectAsync(randomSpecificationObject);

            SpecificationObjectDependencyException actualSpecificationObjectDependencyException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyException>(
                    modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectDependencyException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(randomSpecificationObject.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSpecificationObjectDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(randomSpecificationObject),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedSpecificationObjectException =
                new LockedSpecificationObjectException(
                    message: "Locked dataSetObject record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedSpecificationObjectDependencyValidationException =
                new SpecificationObjectDependencyValidationException(
                    message: "SpecificationObject dependency validation occurred, please try again.",
                    innerException: lockedSpecificationObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.dataSetObjectService.ModifySpecificationObjectAsync(randomSpecificationObject);

            SpecificationObjectDependencyValidationException actualSpecificationObjectDependencyValidationException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyValidationException>(
                    modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectDependencyValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(randomSpecificationObject.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSpecificationObjectDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(randomSpecificationObject),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            var serviceException = new Exception();

            var failedSpecificationObjectServiceException =
                new FailedSpecificationObjectServiceException(
                    message: "Failed dataSetObject service occurred, please contact support", 
                    innerException: serviceException);

            var expectedSpecificationObjectServiceException =
                new SpecificationObjectServiceException(
                    message: "SpecificationObject service error occurred, contact support.",
                    innerException: failedSpecificationObjectServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.dataSetObjectService.ModifySpecificationObjectAsync(randomSpecificationObject);

            SpecificationObjectServiceException actualSpecificationObjectServiceException =
                await Assert.ThrowsAsync<SpecificationObjectServiceException>(
                    modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectServiceException.Should()
                .BeEquivalentTo(expectedSpecificationObjectServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(randomSpecificationObject.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSpecificationObjectServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(randomSpecificationObject),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}