// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Fact]
        public async Task ShouldBulkModifyAddressesAsync()
        {
            // Given
            string randomFilename = GetRandomString();
            string inputFileName = randomFilename;
            List<Address> randomAddresses = new List<Address> { CreateRandomAddress() };
            List<Address> inputAddresses = randomAddresses;

            // When
            await this.addressProcessingService
                .BulkModifyAddressesAsync(addresses: inputAddresses, fileName: inputFileName);

            // Then
            this.addressServiceMock.Verify(service =>
                service.BulkModifyAddressesAsync(inputAddresses, randomFilename),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
