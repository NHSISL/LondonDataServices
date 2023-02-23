using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.IngestionTrackings.Exceptions;
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
                new FailedIngestionTrackingStorageException(sqlException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(failedStorageException);

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
    }
}