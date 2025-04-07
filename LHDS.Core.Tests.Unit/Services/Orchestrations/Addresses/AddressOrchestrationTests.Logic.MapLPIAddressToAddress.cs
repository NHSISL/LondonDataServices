// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldMapLPIAddressToAddress()
        {
            // Given
            var addressOrchestrationServiceMock = new Mock<AddressOrchestrationService>
               (this.addressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.fileBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            string firstUprn = GetRandomString();
            string firstUsrn = GetRandomString();
            string secondUprn = GetRandomString();
            string secondUsrn = GetRandomString();
            string thirdUprn = GetRandomString();
            string thirdUsrn = GetRandomString();

            List<LPIAddress> inputLpiAddresses = new List<LPIAddress>()
            {
                new LPIAddress
                {
                    UPRN = firstUprn,
                    USRN = firstUsrn,
                    SAOStartNumber = "4",
                    SAOStartSuffix = "B",
                    SAOEndNumber = "",
                    SAOEndSuffix = "",
                    SAOText = "",
                    PAOStartNumber = "",
                    PAOStartSuffix = "",
                    PAOEndNumber = "",
                    PAOEndSuffix = "",
                    PAOText = "Fir House"

                },
                new LPIAddress
                {
                    UPRN = secondUprn,
                    USRN = secondUsrn,
                    SAOStartNumber = "1",
                    SAOStartSuffix = "",
                    SAOEndNumber = "4",
                    SAOEndSuffix = "",
                    SAOText = "",
                    PAOStartNumber = "48",
                    PAOStartSuffix = "",
                    PAOEndNumber = "",
                    PAOEndSuffix = "",
                    PAOText = ""
                },
                new LPIAddress
                {
                    UPRN = thirdUprn,
                    USRN = thirdUsrn,
                    SAOStartNumber = "",
                    SAOStartSuffix = "",
                    SAOEndNumber = "",
                    SAOEndSuffix = "",
                    SAOText = "",
                    PAOStartNumber = "",
                    PAOStartSuffix = "",
                    PAOEndNumber = "",
                    PAOEndSuffix = "",
                    PAOText = "Sunshine Palace"
                },
            };

            List<Address> expectedAddresses = new List<Address>()
            {
                new Address
                {
                    UPRN = firstUprn,
                    USRN = firstUsrn,
                    SubBuildingName = "4B",
                    BuildingName = "Fir House",
                    BuildingNumber = ""
                },
                new Address
                {
                    UPRN = secondUprn,
                    USRN = secondUsrn,
                    SubBuildingName = "1-4",
                    BuildingName = "",
                    BuildingNumber = "48"
                },
                new Address
                {
                    UPRN = thirdUprn,
                    USRN = thirdUsrn,
                    SubBuildingName = "",
                    BuildingName = "Sunshine Palace",
                    BuildingNumber = ""
                },
            };

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            List<Address> actualAddresses = [];

            foreach (LPIAddress lpidAddress in inputLpiAddresses)
            {
                Address actualAddress = service.MapLPIAddressToAddress(lpidAddress);
                actualAddresses.Add(actualAddress);
            }

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);
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

