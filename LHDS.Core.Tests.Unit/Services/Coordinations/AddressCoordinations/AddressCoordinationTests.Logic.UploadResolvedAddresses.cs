// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
            Guid expectedBatchReference = Guid.NewGuid();

            this.resolvedAddressOrchestrationServiceMock.Setup(service =>
                service.UploadResolvedAddressesAsync())
                    .ReturnsAsync(expectedBatchReference);

            // When
            Guid? actualBatchReference =
                await this.addressCoordinationService.UploadResolvedAddressesAsync();

            // Then
            actualBatchReference.Should().Be(expectedBatchReference);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.UploadResolvedAddressesAsync(),
                    Times.Once());

            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

