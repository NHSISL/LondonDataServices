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
                new InvalidArgumentAddressPersistenceOrchestrationException(
                    message: "Invalid address persistence orchestration argument, " +
                        "please correct the errors and try again.");

            invalidArgumentAddressPersistanceOrchestrationException.AddData(
                key: "addressList",
                values: "Address list is required");

            invalidArgumentAddressPersistanceOrchestrationException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedAddressPersistanceOrchestrationValidationException =
                new AddressPersistenceOrchestrationValidationException(
                    message: "Address persistence orchestration validation error occurred, please try again",
                    innerException: invalidArgumentAddressPersistanceOrchestrationException);

            // when
            ValueTask<List<Address>> processAddressesTask =
                this.addressPersistanceOrchestrationService.PersistAddressAsync(nullAddressList, invalidFileName);

            AddressPersistenceOrchestrationValidationException
                actualAddressPersistanceOrchestrationValidationException =
                    await Assert.ThrowsAsync<AddressPersistenceOrchestrationValidationException>(
                        processAddressesTask.AsTask);

            //then
            actualAddressPersistanceOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedAddressPersistanceOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressPersistanceOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}