// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
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
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Address> inputAddresses = randomAddresses.DeepClone();
            List<Address> processedAddresses = new List<Address>();

            foreach (Address address in inputAddresses)
            {
                AddressNormalisation addressNormalisation = new AddressNormalisation
                {
                    PostalAddress = GetRandomString(),
                    JsonPostalAddress = GetRandomString()
                };

                var stringAddress = $"{address.OrganisationName},{address.DepartmentName}," +
                    $"{address.SubBuildingName},{address.BuildingName},{address.BuildingNumber}," +
                    $"{address.DependentThoroughfare},{address.Thoroughfare}," +
                    $"{address.DoubleDependentLocality}," +
                    $"{address.DependentLocality},{address.PostTown},{address.PostCode.Replace(" ", "")}";

                this.addressNormalisationProcessingServiceMock.Setup(service =>
                    service.GetNormalisedAddress(stringAddress))
                        .ReturnsAsync(addressNormalisation);

                address.PostalAddress = addressNormalisation.PostalAddress;
                address.JsonPostalAddress = addressNormalisation.JsonPostalAddress;

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
                var stringAddress = $"{address.OrganisationName},{address.DepartmentName}," +
                    $"{address.SubBuildingName},{address.BuildingName},{address.BuildingNumber}," +
                    $"{address.DependentThoroughfare},{address.Thoroughfare}," +
                    $"{address.DoubleDependentLocality}," +
                    $"{address.DependentLocality},{address.PostTown},{address.PostCode.Replace(" ", "")}";

                this.addressNormalisationProcessingServiceMock.Verify(service =>
                    service.GetNormalisedAddress(stringAddress),
                        Times.Once());

                this.addressProcessingServiceMock.Verify(service =>
                    service.ModifyOrAddAddressAsync(It.Is(SameAddressAs(address))),
                        Times.Once());
            }

            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

