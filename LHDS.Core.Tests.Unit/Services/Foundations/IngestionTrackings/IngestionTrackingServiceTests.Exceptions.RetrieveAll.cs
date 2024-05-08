// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedIngestionTrackingStorageException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackings())
                    .Throws(sqlException);

            // when
            Action retrieveAllIngestionTrackingsAction = () =>
                this.ingestionTrackingService.RetrieveAllIngestionTrackings();

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                Assert.Throws<IngestionTrackingDependencyException>(retrieveAllIngestionTrackingsAction);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackings(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedIngestionTrackingServiceException =
                new FailedIngestionTrackingServiceException(
                    message: "Failed ingestion tracking service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingServiceException =
                new IngestionTrackingServiceException(
                    message: "Ingestion tracking service error occurred, please contact support.",
                    innerException: failedIngestionTrackingServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackings())
                    .Throws(serviceException);

            // when
            Action retrieveAllIngestionTrackingsAction = () =>
                this.ingestionTrackingService.RetrieveAllIngestionTrackings();

            IngestionTrackingServiceException actualIngestionTrackingServiceException =
                Assert.Throws<IngestionTrackingServiceException>(retrieveAllIngestionTrackingsAction);

            // then
            actualIngestionTrackingServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackings(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}