// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
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
            Guid actualBatchReference =
                await this.addressClient.ProcessResolvedAddressDataAsync();

            // Then
            actualBatchReference.Should().Be(expectedBatchReference);

            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.UploadResolvedAddressesAsync(),
                    Times.Once());

            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.addressExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
