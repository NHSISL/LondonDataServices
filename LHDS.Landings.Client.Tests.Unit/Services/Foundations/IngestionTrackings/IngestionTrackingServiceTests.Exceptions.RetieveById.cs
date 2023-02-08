// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            string someId = Guid.NewGuid().ToString();
            SqlException sqlException = GetSqlException();

            var failedIngestionTrackingStorageException =
                new FailedIngestionTrackingStorageException(sqlException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(failedIngestionTrackingStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(It.IsAny<string>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IngestionTracking> retrieveIngestionTrackingByIdTask =
                this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(someId);

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyException>(
                    retrieveIngestionTrackingByIdTask.AsTask);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(It.IsAny<string>()),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someId = Guid.NewGuid().ToString();
            var serviceException = new Exception();

            var failedIngestionTrackingServiceException =
                new FailedIngestionTrackingServiceException(serviceException);

            var expectedIngestionTrackingServiceException =
                new IngestionTrackingServiceException(failedIngestionTrackingServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IngestionTracking> retrieveIngestionTrackingByIdTask =
                this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(someId);

            IngestionTrackingServiceException actualIngestionTrackingServiceException =
                await Assert.ThrowsAsync<IngestionTrackingServiceException>(
                    retrieveIngestionTrackingByIdTask.AsTask);

            // then
            actualIngestionTrackingServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(It.IsAny<string>()),
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
