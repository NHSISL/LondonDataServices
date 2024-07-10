// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldSyncAddressesWithAssignAsync()
        {
            // Given

            // When
            await this.addressCoordinationService.SyncAddressesWithAssignAsync();

            // Then

            this.addressOrchestrationServiceMock.Verify(service =>
                service.SyncAddressesWithAssignAsync(),
                    Times.Once());

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

