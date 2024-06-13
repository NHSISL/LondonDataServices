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
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessFileIfArgumentsInvalidAndLogItAsync(string invalidData)
        {
            // given
            Guid invalidSupplierId = Guid.Empty;

            var invalidArgumentEmisLandingCoordinationException =
                new InvalidArgumentEmisLandingCoordinationException(
                    message: "Invalid Emis Landing coordination argument, please correct the errors and try again.");

            invalidArgumentEmisLandingCoordinationException.AddData(
                key: "FileName",
                values: "Text is required");

            invalidArgumentEmisLandingCoordinationException.AddData(
                key: "SupplierId",
                values: "Id is required");

            var expectedEmisLandingCoordinationValidationException =
                new EmisLandingCoordinationValidationException(
                    message: "Emis Landing coordination validation error occurred, please try again.",
                    innerException: invalidArgumentEmisLandingCoordinationException);

            // when
            ValueTask<string> processDataTask = this.emisLandingCoordinationService
                .ProcessFileAsync(ftpFileName: invalidData, supplierId: invalidSupplierId);

            EmisLandingCoordinationValidationException actualEmisLandingCoordinationValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationValidationException>(async () =>
                    await processDataTask);

            // then
            actualEmisLandingCoordinationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmisLandingCoordinationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}