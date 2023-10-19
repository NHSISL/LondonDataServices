// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessAddressesAndLogAsync()
        {
            // Given
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Address> inputAddresses = randomAddresses;

            foreach (Address address in inputAddresses)
            {
                AddressNormalisation addressNormalisation = new AddressNormalisation
                {
                    JsonPostalAddress = address.JsonPostalAddress,
                    PostalAddress = address.PostalAddress,
                };

                var stringAddress = $"{address.OrganisationName},{address.DepartmentName}," +
                    $"{address.SubBuildingName},{address.BuildingName},{address.BuildingNumber}," +
                    $"{address.DependentThoroughfare},{address.Thoroughfare}," +
                    $"{address.DoubleDependentLocality}," +
                    $"{address.DependentLocality},{address.PostTown},{address.PostCode.Replace(" ", "")}";

                this.addressNormalisationProcessingServiceMock.Setup(service =>
                    service.GetNormalisedAddress(stringAddress))
                        .ReturnsAsync(addressNormalisation);

                address.JsonPostalAddress = addressNormalisation.JsonPostalAddress;
                address.PostalAddress = addressNormalisation.PostalAddress;

                this.addressProcessingServiceMock.Setup(service =>
                    service.ModifyOrAddAddressAsync(address))
                        .ReturnsAsync(address);
            }

            List<Address> expectedAddress = inputAddresses.DeepClone();

            // Where
            List<Address> actualAddresses = 
                await this.addressPersistanceOrchestrationService.ProcessAsync(randomAddresses);

            // Then
            actualAddresses.Should().HaveCount(expectedAddress.Count);
            actualAddresses.Should().BeEquivalentTo(expectedAddress);

            foreach(Address address in randomAddresses)
            {
                AddressNormalisation addressNormalisation = new AddressNormalisation
                {
                    JsonPostalAddress = address.JsonPostalAddress,
                    PostalAddress = address.PostalAddress,
                };

                var stringAddress = $"{address.OrganisationName},{address.DepartmentName}," +
                    $"{address.SubBuildingName},{address.BuildingName},{address.BuildingNumber}," +
                    $"{address.DependentThoroughfare},{address.Thoroughfare}," +
                    $"{address.DoubleDependentLocality}," +
                    $"{address.DependentLocality},{address.PostTown},{address.PostCode.Replace(" ", "")}";

                this.addressNormalisationProcessingServiceMock.Verify(service =>
                    service.GetNormalisedAddress(stringAddress),
                        Times.Once());

                address.JsonPostalAddress = addressNormalisation.JsonPostalAddress;
                address.PostalAddress = addressNormalisation.PostalAddress;

                this.addressProcessingServiceMock.Verify(service =>
                    service.ModifyOrAddAddressAsync(address),
                        Times.Once());

                this.addressLoadingAuditProcessingServiceMock.Verify(service =>
                    service.AddAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()),
                        Times.Once());
            }

            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.addressLoadingAuditProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

