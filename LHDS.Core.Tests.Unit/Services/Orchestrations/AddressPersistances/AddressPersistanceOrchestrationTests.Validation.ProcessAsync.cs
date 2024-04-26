// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionsOnProcessIfAddressListIsNullAndLogItAsync(string invalidText)
        {
            // given
            List<Address> nullAddressList = null;
            string invalidFileName = invalidText;

            var invalidArgumentAddressPersistanceOrchestrationException =
                new InvalidArgumentAddressPersistanceOrchestrationException(
                    message: "Invalid address persistance orchestration argument, " +
                        "please correct the errors and try again.");

            invalidArgumentAddressPersistanceOrchestrationException.AddData(
                key: "AddressList",
                values: "Address list is required");

            invalidArgumentAddressPersistanceOrchestrationException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedAddressPersistanceOrchestrationValidationException =
                new AddressPersistanceOrchestrationValidationException(
                    message: "Address persistance orchestration validation error occured, please try again",
                    innerException: invalidArgumentAddressPersistanceOrchestrationException);

            // when
            ValueTask<List<Address>> processAddressesTask =
                this.addressPersistanceOrchestrationService.PersistAddressAsync(nullAddressList, invalidFileName);

            AddressPersistanceOrchestrationValidationException
                actualAddressPersistanceOrchestrationValidationException =
                    await Assert.ThrowsAsync<AddressPersistanceOrchestrationValidationException>(
                        processAddressesTask.AsTask);

            //then
            actualAddressPersistanceOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedAddressPersistanceOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressPersistanceOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}