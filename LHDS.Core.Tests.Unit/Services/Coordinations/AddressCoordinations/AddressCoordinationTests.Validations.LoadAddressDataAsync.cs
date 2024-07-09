// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "empty")]
        [InlineData(" ", "empty")]
        public async Task ShouldThrowValidationExceptionOnLoadAddressDataIfDataIsNullAndLogItAsync(
            string invalidText, string invalidStreamType)
        {
            // given
            Stream invalidStream = invalidStreamType == null ? null : new MemoryStream();
            string invalidFilename = invalidText;

            var invalidArgumentAddressCoordinationException =
                new InvalidArgumentAddressCoordinationException(
                    message: "Invalid address coordination argument, please correct the errors and try again.");

            invalidArgumentAddressCoordinationException.AddData(
                key: "input",
                values: "Stream is required");

            invalidArgumentAddressCoordinationException.AddData(
                key: "filename",
                values: "Text is required");

            var expectedAddressCoordinationValidationException =
                new AddressCoordinationValidationException(
                    message: "Address coordination validation error occurred, please try again.",
                    innerException: invalidArgumentAddressCoordinationException);

            // when
            ValueTask processDataTask =
                this.addressCoordinationService.LoadAddressDataAsync(invalidStream, invalidFilename);

            AddressCoordinationValidationException actualAddressCoordinationValidationException =
                await Assert.ThrowsAsync<AddressCoordinationValidationException>(async () =>
                    await processDataTask);

            // then
            actualAddressCoordinationValidationException.Should()
                .BeEquivalentTo(expectedAddressCoordinationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressCoordinationValidationException))),
                        Times.Once);

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
