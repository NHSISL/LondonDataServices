// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
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
        public async Task
            ShouldThrowValidationExceptionOnRetrieveDownloadByFileNameIfFileNameIsNullAndLogItAsync(string invalidData)
        {
            // given
            Stream invalidStream = null;

            var invalidArgumentEmisLandingCoordinationException =
                new InvalidArgumentEmisLandingCoordinationException(
                    message: "Invalid Emis Landing coordination argument, please correct the errors and try again.");

            invalidArgumentEmisLandingCoordinationException.AddData(
                key: "Output",
                values: "Stream is required");

            invalidArgumentEmisLandingCoordinationException.AddData(
                key: "FileName",
                values:
                [
                    "Text is required",
                    "File name is not valid"
                ]);

            var expectedEmisLandingCoordinationValidationException =
                new EmisLandingCoordinationValidationException(
                    message: "Emis Landing coordination validation error occurred, please try again.",
                    innerException: invalidArgumentEmisLandingCoordinationException);

            // when
            ValueTask retrieveDownloadByFilenameTask =
                this.emisLandingCoordinationService.RetrieveDownloadByFileNameAsync(
                    output: invalidStream,
                    fileName: invalidData);

            EmisLandingCoordinationValidationException actualEmisLandingCoordinationValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationValidationException>(
                    retrieveDownloadByFilenameTask.AsTask);

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