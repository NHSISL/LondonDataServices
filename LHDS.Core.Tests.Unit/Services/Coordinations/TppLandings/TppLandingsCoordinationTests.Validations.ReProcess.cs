// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.TppLandings.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.TppLandings
{
    public partial class TppLandingsCoordinationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnReProcessIfArgumentsInvalidAndLogItAsync()
        {
            // given
            Guid inputSupplierId = Guid.Empty;

            var invalidArgumentTppLandingCoordinationException =
                new InvalidArgumentTppLandingCoordinationException(
                    message: "Invalid TPP landing coordination argument(s), " +
                        "please correct the errors and try again.");

            invalidArgumentTppLandingCoordinationException.AddData(
                key: "SupplierId",
                values: "Id is required");

            var expectedTppLandingCoordinationValidationException =
                new TppLandingCoordinationValidationException(
                    message: "TPP landing coordination validation errors occured, please try again.",
                    innerException: invalidArgumentTppLandingCoordinationException);

            // when
            ValueTask reProcessTask = this.tppLandingCoordinationService.ReProcessAsync(
                supplierId: inputSupplierId);

            TppLandingCoordinationValidationException actualTppLandingCoordinationValidationException =
                await Assert.ThrowsAsync<TppLandingCoordinationValidationException>(reProcessTask.AsTask);

            // then
            actualTppLandingCoordinationValidationException.Should()
               .BeEquivalentTo(expectedTppLandingCoordinationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTppLandingCoordinationValidationException))),
                        Times.Once);

            this.tppLandingOrchestrationServiceMock.Verify(service =>
                service.ReProcessAsync(inputSupplierId),
                Times.Never);

            this.tppLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
