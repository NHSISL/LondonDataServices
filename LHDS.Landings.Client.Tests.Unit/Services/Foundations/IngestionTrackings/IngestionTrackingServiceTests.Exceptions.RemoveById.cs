// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            SqlException sqlException = GetSqlException();

            var failedIngestionTrackingStorageException =
                new FailedIngestionTrackingStorageException(sqlException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(failedIngestionTrackingStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.ReadIngestionTrackingByIdAsync(randomIngestionTracking.Id))
                    .Throws(sqlException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(randomIngestionTracking.Id);

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyException>(
                    addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.ReadIngestionTrackingByIdAsync(randomIngestionTracking.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
