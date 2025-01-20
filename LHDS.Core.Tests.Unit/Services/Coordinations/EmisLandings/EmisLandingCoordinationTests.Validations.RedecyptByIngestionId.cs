// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRedecryptIfIngestionIdIsNullAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;

            var invalidArgumentEmisLandingCoordinationException =
                new InvalidArgumentEmisLandingCoordinationException(
                    message: "Invalid Emis Landing coordination argument, please correct the errors and try again.");

            invalidArgumentEmisLandingCoordinationException.AddData(
                key: "ingestionTrackingId",
                values: "Id is required");

            var expectedEmisLandingCoordinationValidationException =
                new EmisLandingCoordinationValidationException(
                    message: "Emis Landing coordination validation error occurred, please try again.",
                    innerException: invalidArgumentEmisLandingCoordinationException);

            // when
            ValueTask redecryptTask =
                this.emisLandingCoordinationService.RedecryptDocumentByIngestionIdAsync(invalidId);

            EmisLandingCoordinationValidationException actualEmisLandingCoordinationValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationValidationException>(
                    redecryptTask.AsTask);

            // then
            actualEmisLandingCoordinationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEmisLandingCoordinationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }
    }
}