// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByEncryptedFileNameIfSqlErrorOccursAndLogItAsync()
        {
            // given
            string someFileName = GetRandomMessage();
            SqlException sqlException = GetSqlException();

            var failedIngestionTrackingStorageException =
                new FailedIngestionTrackingStorageException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: failedIngestionTrackingStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IngestionTracking> retrieveIngestionTrackingByFileNameTask =
                this.ingestionTrackingService.RetrieveIngestionTrackingByEncryptedFileNameAsync(someFileName);

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyException>(
                    retrieveIngestionTrackingByFileNameTask.AsTask);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByEncryptedFileNameIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someFileName = GetRandomMessage();
            var serviceException = new Exception();

            var failedIngestionTrackingServiceException =
                new FailedIngestionTrackingServiceException(
                    message: "Failed ingestion tracking service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingServiceException =
                new IngestionTrackingServiceException(
                    message: "Ingestion tracking service error occurred, please contact support.",
                    innerException: failedIngestionTrackingServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IngestionTracking> retrieveIngestionTrackingByFileNameTask =
                this.ingestionTrackingService.RetrieveIngestionTrackingByEncryptedFileNameAsync(someFileName);

            IngestionTrackingServiceException actualIngestionTrackingServiceException =
                await Assert.ThrowsAsync<IngestionTrackingServiceException>(
                    retrieveIngestionTrackingByFileNameTask.AsTask);

            // then
            actualIngestionTrackingServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedIngestionTrackingServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
