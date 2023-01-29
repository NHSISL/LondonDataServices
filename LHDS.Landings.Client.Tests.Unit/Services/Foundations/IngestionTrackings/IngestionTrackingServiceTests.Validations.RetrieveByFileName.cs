// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.IngestionTracking.Exceptions;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByFileNameIfFileNameIsInvalidAndLogItAsync()
        {
            // given
            string FileName = String.Empty;

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException();

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.FileName),
                values: "Text is required");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> retrieveIngestionTrackingByFileNameTask =
                this.ingestionTrackingService.RetrieveIngestionTrackingByFileNameAsync(FileName);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    retrieveIngestionTrackingByFileNameTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.ReadIngestionTrackingByFileNameAsync(It.IsAny<string>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByFileNameIfIngestionTrackingIsNotFoundAndLogItAsync()
        {
            //given
            string someFileName = GetRandomMessage();
            IngestionTracking noIngestionTracking = null;

            var notFoundIngestionTrackingForFileNameException =
                new NotFoundIngestionTrackingForFileNameException(someFileName);

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(notFoundIngestionTrackingForFileNameException);

            this.storageBrokerMock.Setup(broker =>
                broker.ReadIngestionTrackingByFileNameAsync(It.IsAny<string>()))
                    .ReturnsAsync(noIngestionTracking);

            //when
            ValueTask<IngestionTracking> retrieveIngestionTrackingByFileNameTask =
                this.ingestionTrackingService.RetrieveIngestionTrackingByFileNameAsync(someFileName);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    retrieveIngestionTrackingByFileNameTask.AsTask);

            //then
            actualIngestionTrackingValidationException.Should().BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.ReadIngestionTrackingByFileNameAsync(It.IsAny<string>()),
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
