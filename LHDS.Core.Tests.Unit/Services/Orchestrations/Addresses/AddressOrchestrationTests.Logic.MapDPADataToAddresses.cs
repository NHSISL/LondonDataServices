// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldMapDPADataToAddressesAsync()
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

            string inputCsvFileName = GetRandomString();

            Func<string, bool> recordFilter = record =>
                record.StartsWith("28,") || record.StartsWith("\"28\",");

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
            {
                { "UPRN", 3 },
                { "UPSN", 4 },
                { "OrganisationName", 5 },
                { "DepartmentName", 6 },
                { "SubBuildingName", 7 },
                { "BuildingName", 8 },
                { "BuildingNumber", 9 },
                { "DependentThoroughfare", 10 },
                { "Thoroughfare", 11 },
                { "DoubleDependentLocality", 12 },
                { "DependentLocality", 13 },
                { "PostTown", 14 },
                { "PostCode", 15 }
            };

            List<Address> randomAddresses = CreateRandomAddresses(count: 2).ToList();
            List<Address> outputAddresses = randomAddresses.DeepClone();
            List<Address> expectedAddresses = outputAddresses.DeepClone();

            addressOrchestrationServiceMock.Setup(service =>
                service.LoadAndMapCsvAsync<Address>(
                    inputCsvFileName,
                    fieldMappings,
                    It.IsAny<Func<string, bool>>()))
                        .ReturnsAsync(outputAddresses);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            List<Address> actualAddresses = await service.MapDPADataToAddressesAsync(inputCsvFileName);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            addressOrchestrationServiceMock.Verify(service =>
                service.LoadAndMapCsvAsync<Address>(
                    inputCsvFileName,
                    fieldMappings,
                    It.IsAny<Func<string, bool>>()),
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

