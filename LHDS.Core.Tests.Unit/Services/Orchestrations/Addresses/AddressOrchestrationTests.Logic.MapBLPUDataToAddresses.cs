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
                this.loggingBrokerMock.Object,
                this.identifierBrokerMock.Object)
            { CallBase = true };

            string inputCsvFileName = GetRandomString();
            int inputBatchSize = 120000;
            int inputSkipCounter = 0;

            Dictionary<string, int> inputFieldMappings = new Dictionary<string, int>
            {
                { "UPRN", 3 },
                { "LogicalStatus", 4 },
                { "StartDate", 15 },
                { "EndDate", 16 },
                { "PostCode", 20 },
                { "YCoordinate", 9 },
                { "XCoordinate", 8 },
                { "Latitude", 10 },
                { "Longitude", 11 },
            };

            string multipleHistoricalsUprn = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset latestEndDate = randomDateTimeOffset.AddDays(5);
            DateTimeOffset olderEndDate = randomDateTimeOffset.AddDays(3);
            DateTimeOffset startDate = randomDateTimeOffset.AddDays(1);
            
            string updatedMultipleHistoricalsPostCode = GetRandomString();
            string updatedMultipleHistoricalsYCoordinate = GetRandomString();
            string updatedMultipleHistoricalsXCoordinate = GetRandomString();
            string updatedMultipleHistoricalsLatitude = GetRandomString();
            string updatedMultipleHistoricalsLongitude = GetRandomString();
            
            string oldMultipleHistoricalsPostCode = GetRandomString();
            string oldMultipleHistoricalsYCoordinate = GetRandomString();
            string oldMultipleHistoricalsXCoordinate = GetRandomString();
            string oldMultipleHistoricalsLatitude = GetRandomString();
            string oldMultipleHistoricalsLongitude = GetRandomString();

            List<BLPUAddress> multipleHistoricalsBlpuAddresses =
            [
                new BLPUAddress
                {
                    UPRN = multipleHistoricalsUprn,
                    LogicalStatus = 8,
                    StartDate = startDate,
                    EndDate = latestEndDate,
                    PostCode = updatedMultipleHistoricalsPostCode,
                    YCoordinate = updatedMultipleHistoricalsYCoordinate,
                    XCoordinate = updatedMultipleHistoricalsXCoordinate,
                    Latitude = updatedMultipleHistoricalsLatitude,
                    Longitude = updatedMultipleHistoricalsLongitude,

                },
                new BLPUAddress
                {
                    UPRN = multipleHistoricalsUprn,
                    LogicalStatus = 8,
                    StartDate = startDate,
                    EndDate = olderEndDate,
                    PostCode = oldMultipleHistoricalsPostCode,
                    YCoordinate = oldMultipleHistoricalsYCoordinate,
                    XCoordinate = oldMultipleHistoricalsXCoordinate,
                    Latitude = oldMultipleHistoricalsLatitude,
                    Longitude = oldMultipleHistoricalsLongitude,
                },
                new BLPUAddress
                {
                    UPRN = multipleHistoricalsUprn,
                    LogicalStatus = 8,
                    StartDate = startDate,
                    EndDate = null,
                    PostCode = oldMultipleHistoricalsPostCode,
                    YCoordinate = oldMultipleHistoricalsYCoordinate,
                    XCoordinate = oldMultipleHistoricalsXCoordinate,
                    Latitude = oldMultipleHistoricalsLatitude,
                    Longitude = oldMultipleHistoricalsLongitude,
                },
            ];

            string multipleAlternativesUprn = GetRandomString();

            string approvedPostCode = GetRandomString();
            string approvedYCoordinate = GetRandomString();
            string approvedXCoordinate = GetRandomString();
            string approvedLatitude = GetRandomString();
            string approvedLongitude = GetRandomString();

            string alternativePostCode = GetRandomString();
            string alternativeYCoordinate = GetRandomString();
            string alternativeXCoordinate = GetRandomString();
            string alternativeLatitude = GetRandomString();
            string alternativeLongitude = GetRandomString();

            List<BLPUAddress> multipleAlternativesBlpuAddresses =
            [
                new BLPUAddress
                {
                    UPRN = multipleAlternativesUprn,
                    LogicalStatus = 1,
                    StartDate = startDate,
                    EndDate = null,
                    PostCode = approvedPostCode,
                    YCoordinate = approvedYCoordinate,
                    XCoordinate = approvedXCoordinate,
                    Latitude = approvedLatitude,
                    Longitude = approvedLongitude,
                },
                new BLPUAddress
                {
                    UPRN = multipleAlternativesUprn,
                    LogicalStatus = 3,
                    StartDate = startDate,
                    EndDate = null,
                    PostCode = alternativePostCode,
                    YCoordinate = alternativeYCoordinate,
                    XCoordinate = alternativeXCoordinate,
                    Latitude = alternativeLatitude,
                    Longitude = alternativeLongitude,
                },
                new BLPUAddress
                {
                    UPRN = multipleAlternativesUprn,
                    LogicalStatus = 3,
                    StartDate = startDate,
                    EndDate = null,
                    PostCode = alternativePostCode,
                    YCoordinate = alternativeYCoordinate,
                    XCoordinate = alternativeXCoordinate,
                    Latitude = alternativeLatitude,
                    Longitude = alternativeLongitude,
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
                    YCoordinate = updatedMultipleHistoricalsYCoordinate,
                    XCoordinate = updatedMultipleHistoricalsXCoordinate,
                    Longitude = updatedMultipleHistoricalsLongitude,
                    Latitude = updatedMultipleHistoricalsLatitude,
                },
                new Address
                {
                    UPRN = multipleAlternativesUprn,
                    PostCode= approvedPostCode,
                    YCoordinate = approvedYCoordinate,
                    XCoordinate = approvedXCoordinate,
                    Longitude = approvedLongitude,
                    Latitude = approvedLatitude,
                },
            ];

            addressOrchestrationServiceMock.Setup(service =>
                service.LoadAndMapCsvAsync<BLPUAddress>(
                    inputCsvFileName,
                    inputFieldMappings,
                    inputBatchSize,
                    inputSkipCounter))
                        .ReturnsAsync(
                            outputBlpuAddresses);

            AddressOrchestrationService service =
                addressOrchestrationServiceMock.Object;

            // When
            List<Address> actualAddresses =
                await service.MapBLPUDataToAddressesAsync(
                    inputCsvFileName,
                    inputBatchSize,
                    inputSkipCounter);

            // Then
            actualAddresses.Should()
                .BeEquivalentTo(expectedAddresses);

            addressOrchestrationServiceMock.Verify(service =>
                service.LoadAndMapCsvAsync<BLPUAddress>(
                    inputCsvFileName,
                    inputFieldMappings,
                    inputBatchSize,
                    inputSkipCounter),
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
