// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldMapBLPUDataToAddressesAsync()
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

            Func<string, bool> inputRecordFilter = record =>
                record.StartsWith("21,") || record.StartsWith("\"21\",");

            Dictionary<string, int> inputFieldMappings = new Dictionary<string, int>
            {
                { "UPRN", 3 },
                { "LogicalStatus", 4 },
                { "StartDate", 15 },
                { "EndDate", 16 },
                { "PostCode", 20 },
            };

            string multipleHistoricalsUprn = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset latestEndDate = randomDateTimeOffset.AddDays(5);
            DateTimeOffset olderEndDate = randomDateTimeOffset.AddDays(3);
            DateTimeOffset startDate = randomDateTimeOffset.AddDays(1);
            string updatedMultipleHistoricalsPostCode = GetRandomString();
            string oldMultipleHistoricalsPostCode = GetRandomString();

            List<BLPUAddress> multipleHistoricalsBlpuAddresses =
            [
                new BLPUAddress
                {
                    UPRN = multipleHistoricalsUprn,
                    LogicalStatus = 8,
                    StartDate = startDate,
                    EndDate = latestEndDate,
                    PostCode = updatedMultipleHistoricalsPostCode
                },
                new BLPUAddress
                {
                    UPRN = multipleHistoricalsUprn,
                    LogicalStatus = 8,
                    StartDate = startDate,
                    EndDate = olderEndDate,
                    PostCode = oldMultipleHistoricalsPostCode
                },
                new BLPUAddress
                {
                    UPRN = multipleHistoricalsUprn,
                    LogicalStatus = 8,
                    StartDate = startDate,
                    EndDate = null,
                    PostCode = oldMultipleHistoricalsPostCode
                },
            ];

            string multipleAlternativesUprn = GetRandomString();
            string approvedPostCode = GetRandomString();
            string alternativePostCode = GetRandomString();

            List<BLPUAddress> multipleAlternativesBlpuAddresses =
            [
                new BLPUAddress
                {
                    UPRN = multipleAlternativesUprn,
                    LogicalStatus = 1,
                    StartDate = startDate,
                    EndDate = null,
                    PostCode = approvedPostCode
                },
                new BLPUAddress
                {
                    UPRN = multipleAlternativesUprn,
                    LogicalStatus = 3,
                    StartDate = startDate,
                    EndDate = null,
                    PostCode = alternativePostCode
                },
                new BLPUAddress
                {
                    UPRN = multipleAlternativesUprn,
                    LogicalStatus = 3,
                    StartDate = startDate,
                    EndDate = null,
                    PostCode = alternativePostCode
                },
            ];

            List<BLPUAddress> outputBlpuAddresses = [
                .. multipleHistoricalsBlpuAddresses,
                .. multipleAlternativesBlpuAddresses];

            List<Address> expectedAddresses =
            [
                new Address
                {
                    UPRN = multipleHistoricalsUprn,
                    PostCode= updatedMultipleHistoricalsPostCode,
                },
                new Address
                {
                    UPRN = multipleAlternativesUprn,
                    PostCode= approvedPostCode,
                },
            ];

            addressOrchestrationServiceMock.Setup(service =>
                service.LoadAndMapCsvAsync<BLPUAddress>(
                    inputCsvFileName,
                    inputFieldMappings,
                    It.IsAny<Func<string, bool>>()))
                        .ReturnsAsync(outputBlpuAddresses);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            List<Address> actualAddresses = await service.MapBLPUDataToAddressesAsync(inputCsvFileName);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            addressOrchestrationServiceMock.Verify(service =>
                service.LoadAndMapCsvAsync<BLPUAddress>(
                    inputCsvFileName,
                    inputFieldMappings,
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
