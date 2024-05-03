// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnMatchAndPersistResolvedAddressAsyncIfResolvedAddressIsNullAndLogItAsync()
        {
            // given
            ResolvedAddress randomResolvedAddress = null;

            var invalidArgumentAddressPersistanceOrchestrationException =
                new InvalidArgumentAddressPersistenceOrchestrationException(
                    message: "Invalid address persistence orchestration argument, " +
                        "please correct the errors and try again.");

            var expectedAddressPersistanceOrchestrationValidationException =
                new AddressPersistenceOrchestrationValidationException(
                    message: "Address persistence orchestration validation error occurred, please try again",
                    innerException: invalidArgumentAddressPersistanceOrchestrationException);

            // when
            ValueTask<ResolvedAddress> processResolvedAddressesTask =
                this.addressPersistanceOrchestrationService.MatchAndPersistResolvedAddressAsync(
                    resolvedAddresses: randomResolvedAddress);

            AddressPersistenceOrchestrationValidationException
                actualAddressPersistanceOrchestrationValidationException =
                    await Assert.ThrowsAsync<AddressPersistenceOrchestrationValidationException>(
                        processResolvedAddressesTask.AsTask);

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