// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public async Task ShouldMapLPIDataToAddresses()
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
            int inputBatchSize = GetRandomNumber();
            int inputSkipCounter = GetRandomNumber();

            Func<string, bool> inputRecordFilter = record =>
                record.StartsWith("24,") || record.StartsWith("\"24\",");

            Dictionary<string, int> inputFieldMappings = new Dictionary<string, int>
            {
                { "UPRN", 3 },
                { "LogicalStatus", 6 },
                { "StartDate", 7 },
                { "EndDate", 8 },
                { "SAOStartNumber", 11 },
                { "SAOStartSuffix", 12 },
                { "SAOEndNumber", 13 },
                { "SAOEndSuffix", 14 },
                { "SAOText", 15 },
                { "PAOStartNumber", 16 },
                { "PAOStartSuffix", 17 },
                { "PAOEndNumber", 18 },
                { "PAOEndSuffix", 19 },
                { "PAOText", 20 },
                { "USRN", 21 },
            };

            string multipleHistoricalsUprn = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset latestEndDate = randomDateTimeOffset.AddDays(5);
            DateTimeOffset olderEndDate = randomDateTimeOffset.AddDays(3);
            DateTimeOffset startDate = randomDateTimeOffset.AddDays(1);
            string updatedMultipleHistoricalsPaoText = GetRandomString();
            string oldMultipleHistoricalsPaoText = GetRandomString();

            List<LPIAddress> multipleHistoricalsLpiAddresses =
            [
                new LPIAddress
                {
                    UPRN = multipleHistoricalsUprn,
                    LogicalStatus = 8,
                    StartDate = startDate,
                    EndDate = latestEndDate,
                    PAOText = updatedMultipleHistoricalsPaoText
                },
                new LPIAddress
                {
                    UPRN = multipleHistoricalsUprn,
                    LogicalStatus = 8,
                    StartDate = startDate,
                    EndDate = olderEndDate,
                    PAOText = oldMultipleHistoricalsPaoText
                },
                new LPIAddress
                {
                    UPRN = multipleHistoricalsUprn,
                    LogicalStatus = 8,
                    StartDate = startDate,
                    EndDate = null,
                    PAOText = oldMultipleHistoricalsPaoText
                },
            ];

            string multipleAlternativesUprn = GetRandomString();
            string approvedPaoText = GetRandomString();
            string alternativePaoText = GetRandomString();

            List<LPIAddress> multipleAlternativesLpiAddresses =
            [
                new LPIAddress
                {
                    UPRN = multipleAlternativesUprn,
                    LogicalStatus = 1,
                    StartDate = startDate,
                    EndDate = null,
                    PAOText = approvedPaoText
                },
                new LPIAddress
                {
                    UPRN = multipleAlternativesUprn,
                    LogicalStatus = 3,
                    StartDate = startDate,
                    EndDate = null,
                    PAOText = alternativePaoText
                },
                new LPIAddress
                {
                    UPRN = multipleAlternativesUprn,
                    LogicalStatus = 3,
                    StartDate = startDate,
                    EndDate = null,
                    PAOText = alternativePaoText
                },
            ];

            List<LPIAddress> outputLpiAddresses = [
                .. multipleHistoricalsLpiAddresses,
                .. multipleAlternativesLpiAddresses];


            List<LPIAddress> inputLpiAddresses =
            [
                new LPIAddress
                {
                    UPRN = multipleHistoricalsUprn,
                    LogicalStatus = 8,
                    StartDate = startDate,
                    EndDate = latestEndDate,
                    PAOText = updatedMultipleHistoricalsPaoText
                },
                new LPIAddress
                {
                    UPRN = multipleAlternativesUprn,
                    LogicalStatus = 1,
                    StartDate = startDate,
                    EndDate = null,
                    PAOText = approvedPaoText
                },
            ];

            List<Address> outputAddresses = new List<Address>()
            {

                new Address
                {
                    UPRN = multipleHistoricalsUprn,
                    SubBuildingName = "",
                    BuildingName = updatedMultipleHistoricalsPaoText,
                    BuildingNumber = ""
                },
                new Address
                {
                    UPRN = multipleAlternativesUprn,
                    SubBuildingName = "",
                    BuildingName = approvedPaoText,
                    BuildingNumber = ""
                },
            };

            List<Address> expectedAddresses = outputAddresses.DeepClone();

            addressOrchestrationServiceMock.Setup(service =>
                service.LoadAndMapCsvAsync<LPIAddress>(
                    inputCsvFileName,
                    inputFieldMappings,
                    inputBatchSize,
                    inputSkipCounter))
                        .ReturnsAsync(outputLpiAddresses);

            for (int i = 0; i < inputLpiAddresses.Count; i++)
            {
                addressOrchestrationServiceMock.Setup(service =>
                    service.MapLPIAddressToAddress(inputLpiAddresses[i]))
                        .Returns(outputAddresses[i]);
            }

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            List<Address> actualAddresses = await service
                .MapLPIDataToAddressesAsync(inputCsvFileName, inputBatchSize, inputSkipCounter);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            addressOrchestrationServiceMock.Verify(service =>
                service.LoadAndMapCsvAsync<LPIAddress>(
                    inputCsvFileName,
                    inputFieldMappings,
                    inputBatchSize,
                    inputSkipCounter),
                        Times.Once);

            for (int i = 0; i < inputLpiAddresses.Count; i++)
            {
                addressOrchestrationServiceMock.Verify(service =>
                    service.MapLPIAddressToAddress(It.Is(SameLPIAddressAs(inputLpiAddresses[i]))),
                        Times.Once);
            }

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

