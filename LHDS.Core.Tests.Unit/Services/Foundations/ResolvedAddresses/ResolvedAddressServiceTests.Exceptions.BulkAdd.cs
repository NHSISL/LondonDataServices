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
        public async Task ShouldThrowCriticalDependencyExceptionOnBulkAddIfSqlErrorOccursAndLogItAsync()
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
                    $"Unable to process resolved addresses in 1 of the batch(es) from {someFileName}",
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
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask addResolvedAddressTask = this.resolvedAddressService
                .BulkAddResolvedAddressesAsync(resolvedAddresses: someResolvedAddresses, fileName: someFileName);

            ResolvedAddressServiceException actualResolvedAddressDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressServiceException>(
                    addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
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
        public async Task ShouldThrowValidationExceptionOnBulkAddIfReferenceErrorOccursAndLogItAsync()
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
                    message: "Invalid resolved address reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var addressDependencyValidationException = new ResolvedAddressDependencyValidationException(
                    message: "Resolved address dependency validation occurred, please try again.",
                    innerException: invalidResolvedAddressReferenceException);

            var aggregateException =
                new AggregateException(
                    $"Unable to process resolved addresses in 1 of the batch(es) from {someFileName}",
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
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask addResolvedAddressTask = this.resolvedAddressService
                .BulkAddResolvedAddressesAsync(resolvedAddresses: someResolvedAddresses, fileName: someFileName);

            // then
            ResolvedAddressServiceException actualResolvedAddressServiceException =
                await Assert.ThrowsAsync<ResolvedAddressServiceException>(
                    addResolvedAddressTask.AsTask);

            actualResolvedAddressServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
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
        public async Task ShouldThrowDependencyExceptionOnBulkAddIfDatabaseUpdateErrorOccursAndLogItAsync()
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
                    $"Unable to process resolved addresses in 1 of the batch(es) from {someFileName}",
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
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask addResolvedAddressTask = this.resolvedAddressService
                .BulkAddResolvedAddressesAsync(resolvedAddresses: someResolvedAddresses, fileName: someFileName);

            ResolvedAddressServiceException actualResolvedAddressServiceException =
                await Assert.ThrowsAsync<ResolvedAddressServiceException>(
                    addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
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
        public async Task ShouldThrowServiceExceptionOnBulkAddIfServiceErrorOccursAndLogItAsync()
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
                .Setup(x =>
                    x.BulkInsertBatch(It.IsAny<List<ResolvedAddress>>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(serviceException);

            ResolvedAddressService addressService = resolvedAddressServiceMock.Object;

            string someFileName = GetRandomString();
            List<ResolvedAddress> someResolvedAddresses = new List<ResolvedAddress> { CreateRandomResolvedAddress() };

            var failedResolvedAddressServiceException =
                new FailedResolvedAddressServiceException(
                    message: "Failed resolved address service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressServiceException =
                new ResolvedAddressServiceException(
                    message: "Resolved address service error occurred, please contact support.",
                    innerException: failedResolvedAddressServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask addResolvedAddressTask = resolvedAddressServiceMock.Object
                .BulkAddResolvedAddressesAsync(resolvedAddresses: someResolvedAddresses, fileName: someFileName);

            ResolvedAddressServiceException actualResolvedAddressServiceException =
                await Assert.ThrowsAsync<ResolvedAddressServiceException>(
                    addResolvedAddressTask.AsTask);

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