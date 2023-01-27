// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedIngestionTrackingStorageException(sqlException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.ReadAllIngestionTracking())
                    .Throws(sqlException);

            // when
            Action retrieveAllIngestionTrackingsAction = () =>
                this.ingestionTrackingService.RetrieveAllIngestionTracking();

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                Assert.Throws<IngestionTrackingDependencyException>(retrieveAllIngestionTrackingsAction);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.ReadAllIngestionTracking(),
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
                new FailedIngestionTrackingServiceException(serviceException);

            var expectedIngestionTrackingServiceException =
                new IngestionTrackingServiceException(failedIngestionTrackingServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.ReadAllIngestionTracking())
                    .Throws(serviceException);

            // when
            Action retrieveAllIngestionTrackingsAction = () =>
                this.ingestionTrackingService.RetrieveAllIngestionTracking();

            IngestionTrackingServiceException actualIngestionTrackingServiceException =
                Assert.Throws<IngestionTrackingServiceException>(retrieveAllIngestionTrackingsAction);

            // then
            actualIngestionTrackingServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.ReadAllIngestionTracking(),
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
