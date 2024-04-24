// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
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
        public async Task ShouldThrowValidationExceptionOnMatchAddressDataIfDataIsNullAndLogItAsync(string invalidText)
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
            this.addressCoordinationService.MatchAddressDataAsync(nullData, invalidFilename);

            AddressCoordinationValidationException actualAddressCoordinationValidationException =
                await Assert.ThrowsAsync<AddressCoordinationValidationException>(async () =>
                    await matchAddressDataTask);

            // then
            actualAddressCoordinationValidationException.Should()
                .BeEquivalentTo(expectedAddressCoordinationValidationException);

            this.addressExtractionOrchestrationServiceMock.Verify(service =>
                service.ProcessResolvedAddressesAsync(nullData, invalidFilename),
                    Times.Never());

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
