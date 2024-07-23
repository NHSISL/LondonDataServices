// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddress someResolvedAddress = CreateRandomResolvedAddress();
            SqlException sqlException = GetSqlException();

            var failedResolvedAddressStorageException =
                new FailedResolvedAddressStorageException(
                    message: "Failed resolved address storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedResolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "Resolved address dependency error occurred, please contact support.",
                    innerException: failedResolvedAddressStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                this.resolvedAddressService.AddResolvedAddressAsync(someResolvedAddress);

            ResolvedAddressDependencyException actualResolvedAddressDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressDependencyException>(
                    addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedResolvedAddressDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfResolvedAddressAlreadyExsitsAndLogItAsync()
        {
            // given
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress alreadyExistsResolvedAddress = randomResolvedAddress;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsResolvedAddressException =
                new AlreadyExistsResolvedAddressException(
                    message: "Resolved address with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedResolvedAddressDependencyValidationException =
                new ResolvedAddressDependencyValidationException(
                    message: "Resolved address dependency validation occurred, please try again.",
                    innerException: alreadyExistsResolvedAddressException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                this.resolvedAddressService.AddResolvedAddressAsync(alreadyExistsResolvedAddress);

            // then
            ResolvedAddressDependencyValidationException actualResolvedAddressDependencyValidationException =
                await Assert.ThrowsAsync<ResolvedAddressDependencyValidationException>(
                    addResolvedAddressTask.AsTask);

            actualResolvedAddressDependencyValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddress someResolvedAddress = CreateRandomResolvedAddress();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidResolvedAddressReferenceException =
                new InvalidResolvedAddressReferenceException(
                    message: "Invalid resolvedAddress reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedResolvedAddressValidationException =
                new ResolvedAddressDependencyValidationException(
                    message: "Resolved address dependency validation occurred, please try again.",
                    innerException: invalidResolvedAddressReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                this.resolvedAddressService.AddResolvedAddressAsync(someResolvedAddress);

            // then
            ResolvedAddressDependencyValidationException actualResolvedAddressDependencyValidationException =
                await Assert.ThrowsAsync<ResolvedAddressDependencyValidationException>(
                    addResolvedAddressTask.AsTask);

            actualResolvedAddressDependencyValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(someResolvedAddress),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddress someResolvedAddress = CreateRandomResolvedAddress();

            var databaseUpdateException =
                new DbUpdateException();

            var failedResolvedAddressStorageException =
                new FailedResolvedAddressStorageException(
                    message: "Failed resolved address storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedResolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "Resolved address dependency error occurred, please contact support.",
                    innerException: failedResolvedAddressStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                this.resolvedAddressService.AddResolvedAddressAsync(someResolvedAddress);

            ResolvedAddressDependencyException actualResolvedAddressDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressDependencyException>(
                    addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddress someResolvedAddress = CreateRandomResolvedAddress();
            var serviceException = new Exception();

            var failedResolvedAddressServiceException =
                new FailedResolvedAddressServiceException(
                    message: "Failed resolved address service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressServiceException =
                new ResolvedAddressServiceException(
                    message: "Resolved address service error occurred, please contact support.",
                    innerException: failedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                this.resolvedAddressService.AddResolvedAddressAsync(someResolvedAddress);

            ResolvedAddressServiceException actualResolvedAddressServiceException =
                await Assert.ThrowsAsync<ResolvedAddressServiceException>(
                    addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}