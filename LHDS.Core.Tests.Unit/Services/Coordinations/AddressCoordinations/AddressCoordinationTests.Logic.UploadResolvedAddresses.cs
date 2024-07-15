// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldUploadResolvedAddressesAsync()
        {
            // Given
            List<Guid> expectedBatchReference = new List<Guid> { Guid.NewGuid() };

            this.resolvedAddressOrchestrationServiceMock.Setup(service =>
                service.ExportResolvedAddressesAsync())
                    .ReturnsAsync(expectedBatchReference);

            // When
            List<Guid> actualBatchReference =
                await this.addressCoordinationService.ExportResolvedAddressesAsync();

            // Then
            actualBatchReference.Should().BeEquivalentTo(expectedBatchReference);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.ExportResolvedAddressesAsync(),
                    Times.Once());

            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

