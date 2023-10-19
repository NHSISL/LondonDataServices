// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnProcessIfAddressListIsNullAndLogItAsync()
        {
            // given
            List<Address> nullAddressList = null;

            var invalidArgumentAddressPersistanceOrchestrationException =
                new InvalidArgumentAddressPersistanceOrchestrationException(
                    message: 
                    "Invalid address persistance orchestration argument, please correct the errors and try again.");

            invalidArgumentAddressPersistanceOrchestrationException.AddData(
                key: "AddressList",
                values: "Address list is required");

            var expectedAddressPersistanceOrchestrationValidationException =
                new AddressPersistanceOrchestrationValidationException(
                    message: "Address persistance orchestration validation errors occured, please try again",
                    innerException: invalidArgumentAddressPersistanceOrchestrationException);

            // when
            ValueTask<List<Address>> processAddressesTask =
                this.addressPersistanceOrchestrationService.ProcessAsync(nullAddressList);

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

            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.addressLoadingAuditProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}