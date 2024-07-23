// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnBulkModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            string someFileName = GetRandomString();
            List<ResolvedAddress> someResolvedAddresses = new List<ResolvedAddress> { CreateRandomResolvedAddress() };
            SqlException sqlException = GetSqlException();

            var failedResolvedAddressStorageException = new FailedResolvedAddressStorageException(
                message: "Failed resolved address storage error occurred, please contact support.",
                innerException: sqlException);

            var addressDependencyException = new ResolvedAddressDependencyException(
                message: "Resolved address dependency error occurred, please contact support.",
                innerException: failedResolvedAddressStorageException);

            var aggregateException =
                new AggregateException(
                    $"Unable to process resolvedAddresses in 1 of the batch(es) from {someFileName}",
                    addressDependencyException);

            var failedResolvedAddressServiceException =
                new FailedResolvedAddressServiceException(
                    message: "Failed aggregate resolved address service error occurred, please contact support.",
                    innerException: sqlException);

            var expectedResolvedAddressServiceException =
                new ResolvedAddressServiceException(
                    message: "Resolved address service error occurred, please contact support.",
                    innerException: failedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask updateResolvedAddressTask = this.resolvedAddressService
                .BulkModifyResolvedAddressesAsync(resolvedAddresses: someResolvedAddresses);

            ResolvedAddressServiceException actualResolvedAddressDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressServiceException>(
                    updateResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    addressDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnBulkModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            string someFileName = GetRandomString();
            List<ResolvedAddress> someResolvedAddresses = new List<ResolvedAddress> { CreateRandomResolvedAddress() };
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidResolvedAddressReferenceException =
                new InvalidResolvedAddressReferenceException(
                    message: "Invalid resolvedAddress reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var addressDependencyValidationException = new ResolvedAddressDependencyValidationException(
                    message: "Resolved address dependency validation occurred, please try again.",
                    innerException: invalidResolvedAddressReferenceException);

            var aggregateException =
                new AggregateException(
                    $"Unable to process resolvedAddresses in 1 of the batch(es) from {someFileName}",
                    addressDependencyValidationException);

            var failedResolvedAddressServiceException =
                new FailedResolvedAddressServiceException(
                    message: "Failed aggregate resolved address service error occurred, please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressServiceException =
                new ResolvedAddressServiceException(
                    message: "Resolved address service error occurred, please contact support.",
                    innerException: failedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask updateResolvedAddressTask = this.resolvedAddressService
                .BulkModifyResolvedAddressesAsync(resolvedAddresses: someResolvedAddresses);

            // then
            ResolvedAddressServiceException actualResolvedAddressServiceException =
                await Assert.ThrowsAsync<ResolvedAddressServiceException>(
                    updateResolvedAddressTask.AsTask);

            actualResolvedAddressServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    addressDependencyValidationException))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualResolvedAddressServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnBulkModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            string someFileName = GetRandomString();
            List<ResolvedAddress> someResolvedAddresses = new List<ResolvedAddress> { CreateRandomResolvedAddress() };

            var databaseUpdateException =
                new DbUpdateException();

            var failedResolvedAddressStorageException =
                new FailedResolvedAddressStorageException(
                    message: "Failed resolved address storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var addressDependencyException = new ResolvedAddressDependencyException(
                    message: "Resolved address dependency error occurred, please contact support.",
                    innerException: failedResolvedAddressStorageException);

            var aggregateException =
                new AggregateException(
                    $"Unable to process resolvedAddresses in 1 of the batch(es) from {someFileName}",
                    addressDependencyException);

            var failedResolvedAddressServiceException =
                new FailedResolvedAddressServiceException(
                    message: "Failed aggregate resolved address service error occurred, please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressServiceException =
                new ResolvedAddressServiceException(
                    message: "Resolved address service error occurred, please contact support.",
                    innerException: failedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask updateResolvedAddressTask = this.resolvedAddressService
                .BulkModifyResolvedAddressesAsync(resolvedAddresses: someResolvedAddresses);

            ResolvedAddressServiceException actualResolvedAddressServiceException =
                await Assert.ThrowsAsync<ResolvedAddressServiceException>(
                    updateResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    addressDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnBulkModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();
            var resolvedAddressServiceMock = new Mock<ResolvedAddressService>(
                this.storageBrokerMock.Object,
                this.identifierBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressServiceMock
                .Setup(service =>
                    service.BulkInsertBatch(It.IsAny<List<ResolvedAddress>>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(serviceException);

            ResolvedAddressService addressService = resolvedAddressServiceMock.Object;

            string someFileName = GetRandomString();
            List<ResolvedAddress> someResolvedAddresses = new List<ResolvedAddress> { CreateRandomResolvedAddress() };

            var failedResolvedAddressServiceException =
                new FailedResolvedAddressServiceException(
                    message: "Failed aggregate resolved address service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressServiceException =
                new ResolvedAddressServiceException(
                    message: "Resolved address service error occurred, please contact support.",
                    innerException: failedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask updateResolvedAddressTask = resolvedAddressServiceMock.Object
                .BulkModifyResolvedAddressesAsync(resolvedAddresses: someResolvedAddresses);

            ResolvedAddressServiceException actualResolvedAddressServiceException =
                await Assert.ThrowsAsync<ResolvedAddressServiceException>(
                    updateResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressServiceException);

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