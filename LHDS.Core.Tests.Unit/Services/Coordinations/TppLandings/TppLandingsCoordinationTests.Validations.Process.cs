// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.TppLandings.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class TppLandingsCoordinationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessIfArgumentsInvalidAndLogItAsync(string invalidText)
        {
            // given
            Guid inputSupplierId = Guid.Empty;
            Stream randomStream = new MemoryStream();
            Stream inputStream = new MemoryStream();
            string inputFileName = invalidText;

            var invalidArgumentTppLandingCoordinationException =
                new InvalidArgumentTppLandingCoordinationException(
                    message: "Invalid TPP landing coordination argument(s), " +
                        "please correct the errors and try again.");

            invalidArgumentTppLandingCoordinationException.AddData(
                key: "Input",
                values: "Stream is required");

            invalidArgumentTppLandingCoordinationException.AddData(
                key: "FileName",
                values: "Text is required");

            invalidArgumentTppLandingCoordinationException.AddData(
                key: "SupplierId",
                values: "Id is required");

            var expectedTppLandingCoordinationValidationException =
                new TppLandingCoordinationValidationException(
                    message: "TPP landing coordination validation errors occured, please try again.",
                    innerException: invalidArgumentTppLandingCoordinationException);

            // when
            ValueTask<Guid> returnedGuidTask = this.tppLandingCoordinationService.ProcessAsync(
                input: inputStream,
                fileName: inputFileName,
                supplierId: inputSupplierId);

            TppLandingCoordinationValidationException actualTppLandingCoordinationValidationException =
                await Assert.ThrowsAsync<TppLandingCoordinationValidationException>(returnedGuidTask.AsTask);

            // then
            actualTppLandingCoordinationValidationException.Should()
               .BeEquivalentTo(expectedTppLandingCoordinationValidationException);

            this.tppLandingOrchestrationServiceMock.Verify(service =>
                service.ProcessAsync(inputStream, inputFileName, inputSupplierId),
                Times.Never);

            this.ingressOrchestrationServiceMock.Verify(service =>
                service.CheckForTPPBatchCompleteAsync(inputFileName),
                    Times.Never);

            this.tppLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
