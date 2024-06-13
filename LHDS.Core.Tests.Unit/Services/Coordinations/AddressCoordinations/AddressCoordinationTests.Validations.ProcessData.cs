// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using LHDS.Core.Models.Foundations.Addresses;
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
        public async Task ShouldThrowValidationExceptionOnProcessDataIfDataIsNullAndLogItAsync(string invalidText)
        {
            // given
            byte[] nullData = null;
            string invalidFilename = invalidText;

            var invalidArgumentAddressCoordinationException =
                new InvalidArgumentAddressCoordinationException(
                    message: "Invalid address coordination argument, please correct the errors and try again.");

            invalidArgumentAddressCoordinationException.AddData(
                key: "data",
                values: "Data is required");

            invalidArgumentAddressCoordinationException.AddData(
                key: "filename",
                values: "Text is required");

            var expectedAddressCoordinationValidationException =
                new AddressCoordinationValidationException(
                    message: "Address coordination validation error occurred, please try again.",
                    innerException: invalidArgumentAddressCoordinationException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressCoordinationService.LoadAddressDataAsync(nullData, invalidFilename);

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

            this.addressExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
