// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldProcessAddressesAndLogAsync()
        {
            // Given
            List<Address> randomAddresses = CreateRandomAddresses(GetRandomNumber()).ToList();
            List<Address> inputAddresses = randomAddresses.DeepClone();
            List<Address> processedAddresses = new List<Address>();

            foreach (Address address in inputAddresses)
            {
                this.addressProcessingServiceMock.Setup(service =>
                    service.ModifyOrAddAddressAsync(address))
                        .ReturnsAsync(address);

                processedAddresses.Add(address);
            }

            List<Address> expectedAddress = processedAddresses.DeepClone();

            // Where
            List<Address> actualAddresses =
                await this.addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses);

            // Then
            actualAddresses.Should().HaveCount(expectedAddress.Count);

            foreach (Address address in inputAddresses)
            {
                this.addressProcessingServiceMock.Verify(service =>
                    service.ModifyOrAddAddressAsync(It.Is(SameAddressAs(address))),
                        Times.Once());
            }

            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

