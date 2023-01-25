// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Reflection.Metadata;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Landings.Client.Models.IngestionTracking;
using LHDS.Landings.Client.Models.IngestionTracking.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();
            SqlException sqlException = GetSqlException();

            var failedIngestionTrackingStorageException =
                new FailedIngestionTrackingStorageException(sqlException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(failedIngestionTrackingStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(someIngestionTracking);

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyException>(
                    addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfIngestionTrackingAlreadyExsitsAndLogItAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking alreadyExistsIngestionTracking = randomIngestionTracking;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsIngestionTrackingException =
                new AlreadyExistsIngestionTrackingException(duplicateKeyException);

            var expectedIngestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(alreadyExistsIngestionTrackingException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(alreadyExistsIngestionTracking);

            // then
            IngestionTrackingDependencyValidationException actualIngestionTrackingDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyValidationException>(
                    addIngestionTrackingTask.AsTask);

            actualIngestionTrackingDependencyValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
