// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowAggregateExceptionOnReadCsvDataIfErrorsInLoadCallsAsync()
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

            string dpaCsvFilePath = "ID28.csv";
            string lpiCsvFilePath = "ID24.csv";
            string blpuCsvFilePath = "ID21.csv";
            string streetDescriptorCsvFilePath = "ID15.csv";

            List<string> fileList = [
                dpaCsvFilePath,
                lpiCsvFilePath,
                blpuCsvFilePath,
                streetDescriptorCsvFilePath];

            string inputTempPath = GetRandomString();
            string inputSearchPattern = "*.csv";
            Xeption dpaException = new Xeption();
            Xeption lpiException = new Xeption();
            Xeption blpuException = new Xeption();
            Xeption streetDescriptorException = new Xeption();

            this.fileBrokerMock.Setup(service =>
                service.GetListOfFilesAsync(inputTempPath, inputSearchPattern))
                    .ReturnsAsync(fileList);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessDPAAddressesAsync(dpaCsvFilePath))
                    .ThrowsAsync(dpaException);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessLPIAddressesAsync(lpiCsvFilePath))
                    .ThrowsAsync(lpiException);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessBLPUAddressesAsync(blpuCsvFilePath))
                    .ThrowsAsync(blpuException);

            addressOrchestrationServiceMock.Setup(service =>
                service.ProcessStreetDescriptorDataAsync(streetDescriptorCsvFilePath))
                    .ThrowsAsync(streetDescriptorException);

            Xeption expectedDpaException = new Xeption();
            Xeption expectedLpiException = new Xeption();
            Xeption expectedBlpuException = new Xeption();
            Xeption expectedStreetDescriptorException = new Xeption();
            expectedDpaException.AddData("DpaExtractionError", dpaCsvFilePath);
            expectedLpiException.AddData("LpiExtractionError", lpiCsvFilePath);
            expectedBlpuException.AddData("BlpuExtractionError", blpuCsvFilePath);

            expectedStreetDescriptorException
                .AddData("StreetDescriptorExtractionError", streetDescriptorCsvFilePath);

            List<Exception> expectedExceptions = [
                expectedDpaException,
                expectedLpiException,
                expectedBlpuException,
                expectedStreetDescriptorException];

            var expectedAggregateException =
                new AggregateException(
                    message: $"Unable to extract {expectedExceptions.Count} address files.",
                    expectedExceptions);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            ValueTask readCsvDataTask = service.ReadCsvDataAndBulkAddAddressesAsync(inputTempPath);

            AggregateException actualAggregateException =
                await Assert.ThrowsAsync<AggregateException>(
                    readCsvDataTask.AsTask);

            // Then
            actualAggregateException.Should().BeEquivalentTo(expectedAggregateException);

            this.fileBrokerMock.Verify(service =>
                service.GetListOfFilesAsync(inputTempPath, inputSearchPattern),
                    Times.Once());

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessDPAAddressesAsync(dpaCsvFilePath),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessLPIAddressesAsync(lpiCsvFilePath),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessBLPUAddressesAsync(blpuCsvFilePath),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.ProcessStreetDescriptorDataAsync(streetDescriptorCsvFilePath),
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

