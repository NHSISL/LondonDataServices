// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldProcessDataAndLogAsync()
        {
            // Given
            string someFilename = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(GetRandomString());
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Address> extractedAddresses = randomAddresses.DeepClone();

            this.addressExtractionOrchestrationServiceMock.Setup(service =>
                service.BulkAddAddressesAsync(inputData, someFilename))
                    .ReturnsAsync(extractedAddresses);

            List<Address> expectedAddresses = extractedAddresses.DeepClone();

            // When
            List<Address> actualAddresses =
                await this.addressCoordinationService.LoadAddressDataAsync(inputData, someFilename);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.addressExtractionOrchestrationServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(inputData, someFilename),
                    Times.Once());

            this.addressExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

