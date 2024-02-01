// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressResolvings
{
    public partial class AddressResolvingOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldResolveAddressesAndLogAsync()
        {
            // Given
            // Step 1 - Add to processing Address
            this.addressProcessingServiceMock.Setup(processing =>
                processing.AddAddressAsync(inputAddress))
                    .ReturnsAsync(storageAddress);

            // Step 2 - Matcher processing
            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.ExtractPostCode(inputAddress))
                    .ReturnsAsync(storagePostcode);

            // Step 3 - Resolved Address
            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.ModifyOrAddResolvedAddressAsync(inputAddress))
                    .ReturnsAsync(storagePostcode);

            // When

            // Then
        }
    }
}

