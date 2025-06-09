// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnLoadAddressDataIfDataIsNullAndLogItAsyncWithFolderPath(
            string invalidText)
        {
            // given
            string invalidFolderPath = invalidText;

            var invalidArgumentAddressCoordinationException =
                new InvalidArgumentAddressCoordinationException(
                    message: "Invalid address coordination argument, please correct the errors and try again.");

            invalidArgumentAddressCoordinationException.AddData(
                key: "folderPath",
                values: "Text is required");

            var expectedAddressCoordinationValidationException =
                new AddressCoordinationValidationException(
                    message: "Address coordination validation error occurred, please try again.",
                    innerException: invalidArgumentAddressCoordinationException);

            // when
            ValueTask processDataTask =
                this.addressCoordinationService.LoadAddressDataAsync(invalidFolderPath);

            AddressCoordinationValidationException actualAddressCoordinationValidationException =
                await Assert.ThrowsAsync<AddressCoordinationValidationException>(
                    processDataTask.AsTask);

            // then
            actualAddressCoordinationValidationException.Should()
                .BeEquivalentTo(expectedAddressCoordinationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressCoordinationValidationException))),
                        Times.Once);

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
