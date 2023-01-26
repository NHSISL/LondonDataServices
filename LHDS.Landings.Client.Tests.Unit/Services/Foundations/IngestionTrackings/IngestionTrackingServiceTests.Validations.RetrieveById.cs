// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.IngestionTracking;
using LHDS.Landings.Client.Models.Foundations.IngestionTracking.Exceptions;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidIngestionTrackingId = Guid.Empty;

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException();

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.Id),
                values: "Id is required");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> retrieveIngestionTrackingByIdTask =
                this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(invalidIngestionTrackingId);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    retrieveIngestionTrackingByIdTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.ReadIngestionTrackingByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfIngestionTrackingIsNotFoundAndLogItAsync()
        {
            //given
            Guid someIngestionTrackingId = Guid.NewGuid();
            IngestionTracking noIngestionTracking = null;

            var notFoundIngestionTrackingException =
                new NotFoundIngestionTrackingException(someIngestionTrackingId);

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(notFoundIngestionTrackingException);

            this.storageBrokerMock.Setup(broker =>
                broker.ReadIngestionTrackingByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noIngestionTracking);

            //when
            ValueTask<IngestionTracking> retrieveIngestionTrackingByIdTask =
                this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(someIngestionTrackingId);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    retrieveIngestionTrackingByIdTask.AsTask);

            //then
            actualIngestionTrackingValidationException.Should().BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.ReadIngestionTrackingByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
