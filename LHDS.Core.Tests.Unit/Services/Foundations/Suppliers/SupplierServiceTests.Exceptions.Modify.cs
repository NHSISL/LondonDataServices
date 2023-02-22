using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Suppliers;
using LHDS.Core.Models.Suppliers.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Supplier randomSupplier = CreateRandomSupplier();
            SqlException sqlException = GetSqlException();

            var failedSupplierStorageException =
                new FailedSupplierStorageException(sqlException);

            var expectedSupplierDependencyException =
                new SupplierDependencyException(failedSupplierStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(randomSupplier);

            SupplierDependencyException actualSupplierDependencyException =
                await Assert.ThrowsAsync<SupplierDependencyException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierDependencyException.Should()
                .BeEquivalentTo(expectedSupplierDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSupplierDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(randomSupplier),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Supplier someSupplier = CreateRandomSupplier();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidSupplierReferenceException =
                new InvalidSupplierReferenceException(foreignKeyConstraintConflictException);

            SupplierDependencyValidationException expectedSupplierDependencyValidationException =
                new SupplierDependencyValidationException(invalidSupplierReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(someSupplier);

            SupplierDependencyValidationException actualSupplierDependencyValidationException =
                await Assert.ThrowsAsync<SupplierDependencyValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierDependencyValidationException.Should()
                .BeEquivalentTo(expectedSupplierDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(someSupplier.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSupplierDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(someSupplier),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Supplier randomSupplier = CreateRandomSupplier();
            var databaseUpdateException = new DbUpdateException();

            var failedSupplierStorageException =
                new FailedSupplierStorageException(databaseUpdateException);

            var expectedSupplierDependencyException =
                new SupplierDependencyException(failedSupplierStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(randomSupplier);

            SupplierDependencyException actualSupplierDependencyException =
                await Assert.ThrowsAsync<SupplierDependencyException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierDependencyException.Should()
                .BeEquivalentTo(expectedSupplierDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(randomSupplier),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            Supplier randomSupplier = CreateRandomSupplier();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedSupplierException =
                new LockedSupplierException(databaseUpdateConcurrencyException);

            var expectedSupplierDependencyValidationException =
                new SupplierDependencyValidationException(lockedSupplierException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(randomSupplier);

            SupplierDependencyValidationException actualSupplierDependencyValidationException =
                await Assert.ThrowsAsync<SupplierDependencyValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierDependencyValidationException.Should()
                .BeEquivalentTo(expectedSupplierDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(randomSupplier),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Supplier randomSupplier = CreateRandomSupplier();
            var serviceException = new Exception();

            var failedSupplierServiceException =
                new FailedSupplierServiceException(serviceException);

            var expectedSupplierServiceException =
                new SupplierServiceException(failedSupplierServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(randomSupplier);

            SupplierServiceException actualSupplierServiceException =
                await Assert.ThrowsAsync<SupplierServiceException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierServiceException.Should()
                .BeEquivalentTo(expectedSupplierServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(randomSupplier),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}