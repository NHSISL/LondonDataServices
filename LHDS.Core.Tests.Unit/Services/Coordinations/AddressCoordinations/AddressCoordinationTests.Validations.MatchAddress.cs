// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Storages.StorageQueues;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnMatchAddressDataFromStreamIfPayloadIsNullAndLogItAsync()
        {
            // given
            Payload<Guid> nullPayload = null;

            var invalidArgumentAddressCoordinationException =
                new InvalidArgumentAddressCoordinationException(
                    message: "Invalid address coordination argument, please correct the errors and try again.");

            invalidArgumentAddressCoordinationException.AddData(
                key: nameof(Payload<Guid>),
                values: "Payload is required");

            var expectedAddressCoordinationValidationException =
                new AddressCoordinationValidationException(
                    message: "Address coordination validation error occurred, please try again.",
                    innerException: invalidArgumentAddressCoordinationException);

            // when
            ValueTask matchAddressDataTask =
                this.addressCoordinationService.MatchAddressDataAsync(payload: nullPayload);

            AddressCoordinationValidationException actualAddressCoordinationValidationException =
                await Assert.ThrowsAsync<AddressCoordinationValidationException>(
                    matchAddressDataTask.AsTask);

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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnMatchAddressDataFromStreamIfMessageIsNullAndLogItAsync()
        {
            // given
            Payload<Guid> invalidPayload = new()
            {
                Message = default,
                EnqueuedAtUtc = DateTimeOffset.UtcNow,
                User = CreateRandomEntraUser()
            };

            var invalidArgumentAddressCoordinationException =
                new InvalidArgumentAddressCoordinationException(
                    message: "Invalid address coordination argument, please correct the errors and try again.");

            invalidArgumentAddressCoordinationException.AddData(
                key: nameof(Payload<Guid>.Message),
                values: "Id is required");

            var expectedAddressCoordinationValidationException =
                new AddressCoordinationValidationException(
                    message: "Address coordination validation error occurred, please try again.",
                    innerException: invalidArgumentAddressCoordinationException);

            // when
            ValueTask matchAddressDataTask =
                this.addressCoordinationService.MatchAddressDataAsync(payload: invalidPayload);

            AddressCoordinationValidationException actualAddressCoordinationValidationException =
                await Assert.ThrowsAsync<AddressCoordinationValidationException>(
                    matchAddressDataTask.AsTask);

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
