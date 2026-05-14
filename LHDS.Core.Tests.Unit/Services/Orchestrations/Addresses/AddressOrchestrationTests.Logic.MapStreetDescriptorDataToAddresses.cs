// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.Addresses;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldMapStreetDescriptorDataToAddressesAsync()
        {
            // Given
            var addressOrchestrationServiceMock = new Mock<AddressOrchestrationService>
               (this.addressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.fileBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.identifierBrokerMock.Object)
            { CallBase = true };

            string inputCsvFileName = GetRandomString();

            Dictionary<string, int> inputFieldMappings = new Dictionary<string, int>
            {
                { "USRN", 3 },
                { "StreetDescription", 4 },
                { "Locality", 5 },
                { "TownName", 6 },
            };

            List<StreetDescriptor> randomStreetDescriptors = CreateRandomStreetDescriptors(count: 2);
            List<StreetDescriptor> outputStreetDescriptors = randomStreetDescriptors.DeepClone();
            List<Address> expectedAddresses = [];

            foreach (StreetDescriptor streetDescriptor in outputStreetDescriptors)
            {
                Address address = new Address
                {
                    USRN = streetDescriptor.USRN,
                    Thoroughfare = streetDescriptor.StreetDescription,
                    DependentLocality = streetDescriptor.Locality,
                    PostTown = streetDescriptor.TownName
                };

                expectedAddresses.Add(address);
            };

            addressOrchestrationServiceMock.Setup(service =>
                service.LoadAndMapCsvAsync<StreetDescriptor>(
                    inputCsvFileName,
                    inputFieldMappings,
                    default))
                        .Returns(
                            outputStreetDescriptors
                                .ToAsyncEnumerable());

            AddressOrchestrationService service =
                addressOrchestrationServiceMock.Object;

            // When
            List<Address> actualAddresses =
                new List<Address>();

            await foreach (Address address in service
                .MapStreetDescriptorDataToAddressesAsync(
                    inputCsvFileName))
            {
                actualAddresses.Add(address);
            }

            // Then
            actualAddresses.Should()
                .BeEquivalentTo(expectedAddresses);

            addressOrchestrationServiceMock.Verify(service =>
                service.LoadAndMapCsvAsync<StreetDescriptor>(
                    inputCsvFileName,
                    inputFieldMappings,
                    default),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}

