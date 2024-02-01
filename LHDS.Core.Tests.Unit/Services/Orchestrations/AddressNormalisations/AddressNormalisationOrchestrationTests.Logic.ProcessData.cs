// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressNormalisations
{
    public partial class AddressNormalisationOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldProcessFileAndNormaliseAndLogAsync()
        {
            // Given
            string inputData = GetRandomString();
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Address> inputAddresses = randomAddresses.DeepClone();

            List<Address> processedAddresses =
                new List<Address>();

            this.addressParserProcessingServiceMock.Setup(processing =>
              processing.ProcessCsvAsync(inputData))
                   .ReturnsAsync(randomAddresses);

            foreach (Address address in inputAddresses)
            {
                AddressNormalisation addressNormalisation = new AddressNormalisation
                {
                    PostalAddress = GetRandomString(),
                    JsonPostalAddress = GetRandomString(),
                    AddressComponents = GenerateKeyValuePairList(GetRandomNumber())
                };

                List<string> addressList = new List<string> {
                    address.OrganisationName,
                    address.DepartmentName,
                    address.SubBuildingName,
                    address.BuildingName,
                    address.BuildingNumber,
                    address.DependentThoroughfare,
                    address.Thoroughfare,
                    address.DoubleDependentLocality,
                    address.DependentLocality,
                    address.PostTown,
                    address.PostCode
                };

                var stringAddress = string.Join("", addressList.Where(s => !string.IsNullOrEmpty(s)));

                this.addressNormalisationProcessingServiceMock.Setup(service =>
                    service.GetNormalisedAddress(stringAddress))
                        .ReturnsAsync(addressNormalisation);

                processedAddresses.Add(address);
            }

            List<Address> expectedAddress = processedAddresses.DeepClone();

            // Where
            List<AddressNormalisation> actualAddresses =
                 await this.addressNormalisationOrchestrationService.ProcessDataAsync(inputData);

            // Then
            actualAddresses.Should().HaveCount(expectedAddress.Count);

            this.addressParserProcessingServiceMock.Verify(processing =>
                   processing.ProcessCsvAsync(It.IsAny<string>()),
                       Times.Once);

            foreach (Address address in inputAddresses)
            {
                List<string> addressList = new List<string> {
                    address.OrganisationName,
                    address.DepartmentName,
                    address.SubBuildingName,
                    address.BuildingName,
                    address.BuildingNumber,
                    address.DependentThoroughfare,
                    address.Thoroughfare,
                    address.DoubleDependentLocality,
                    address.DependentLocality,
                    address.PostTown,
                    address.PostCode
                };

                var stringAddress = string.Join("", addressList.Where(s => !string.IsNullOrEmpty(s)));

                this.addressNormalisationProcessingServiceMock.Verify(service =>
                    service.GetNormalisedAddress(stringAddress),
                        Times.Once());
            }

            this.addressLoadingAuditProcessingServiceMock.Verify(service =>
                service.AddAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()),
                    Times.Exactly(inputAddresses.Count));

            this.addressParserProcessingServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressLoadingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}