// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ResolvedAddresses
{
    public partial class ResolvedAddressProcessingServiceTests
    {
        [Fact]
        public async Task ShouldBulkModifyResolvedAddressesAsync()
        {
            // Given
            List<ResolvedAddress> randomResolvedAddresses = new List<ResolvedAddress> { CreateRandomResolvedAddress() };
            List<ResolvedAddress> inputResolvedAddresses = randomResolvedAddresses;

            // When
            await this.resolvedAddressProcessingService
                .BulkModifyResolvedAddressesAsync(resolvedAddresses: inputResolvedAddresses);

            // Then
            this.resolvedAddressServiceMock.Verify(service =>
                service.BulkModifyResolvedAddressesAsync(inputResolvedAddresses),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}