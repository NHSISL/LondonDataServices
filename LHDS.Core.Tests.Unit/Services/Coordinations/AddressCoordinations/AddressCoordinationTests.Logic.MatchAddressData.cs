// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Storages.StorageQueues;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldMatchAddressDataAsync()
        {
            // Given

            // When
            await this.addressCoordinationService.MatchAddressDataAsync();

            // Then
            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.MatchAddressDataAsync(),
                    Times.Once());

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldMatchAddressAsync()
        {
            // Given
            Guid randomResolvedAddressId = Guid.NewGuid();

            Payload<Guid> inputPayload = new Payload<Guid>
            {
                Message = randomResolvedAddressId,
                EnqueuedAtUtc = DateTimeOffset.UtcNow,
                User = CreateRandomEntraUser()
            };

            // When
            await this.addressCoordinationService.MatchAddressDataAsync(payload: inputPayload);

            // Then
            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.MatchAddressDataAsync(inputPayload),
                    Times.Once());

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

