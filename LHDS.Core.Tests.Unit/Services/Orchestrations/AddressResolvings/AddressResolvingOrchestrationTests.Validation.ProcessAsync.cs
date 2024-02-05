// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Orchestrations.AddressResolvings.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressResolvings
{
    public partial class AddressResolvingOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnProcessIfAddressListIsNullAndLogItAsync()
        {
            // given
            AddressNormalisation nullAddressList = null;

            var invalidArgumentAddressResolvingOrchestrationException =
                new InvalidArgumentAddressResolvingOrchestrationException(
                    message: "Invalid normalised address resolving orchestration argument, " + 
                        "please correct the errors and try again.");

            invalidArgumentAddressResolvingOrchestrationException.AddData(
                key: "NormalisedAddressList",
                values: "Normalised address list is required");

            var expectedAddressResolvingOrchestrationValidationException =
                new AddressResolvingOrchestrationValidationException(
                    message: "Normalised address resolving orchestration validation error occured, please try again",
                    innerException: invalidArgumentAddressResolvingOrchestrationException);

            // when
            ValueTask<AddressNormalisation> processAddressesTask =
                this.addressResolvingOrchestrationService.ResolvedAddressAsync(nullAddressList);

            AddressResolvingOrchestrationValidationException
                actualAddressResolvingOrchestrationValidationException =
                    await Assert.ThrowsAsync<AddressResolvingOrchestrationValidationException>(
                        processAddressesTask.AsTask);

            //then
            actualAddressResolvingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedAddressResolvingOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressResolvingOrchestrationValidationException))),
                        Times.Once);

            addressProcessingServiceMock.VerifyNoOtherCalls();
            addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
            dateTimeBrokerMock.VerifyNoOtherCalls();
            serializationBrokerMock.VerifyNoOtherCalls();
        }
    }
}