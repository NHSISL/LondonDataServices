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
        public async Task ShouldThrowAggregateExceptionOnProcessBLPUIfErrorsInMapCallsAsync()
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

            string blpuCsvFilePath = "ID24.csv";
            Guid randomGuid = Guid.NewGuid();
            Guid inputCorrelationId = randomGuid;
            Xeption blpuException = new Xeption();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(inputCorrelationId);

            addressOrchestrationServiceMock.Setup(service =>
                service.MapBLPUDataToAddressesAsync(
                    blpuCsvFilePath,
                    default))
                        .Throws(blpuException);

            AddressOrchestrationService service =
                addressOrchestrationServiceMock.Object;

            // When
            ValueTask readCsvDataTask =
                service.ProcessBLPUAddressesAsync(blpuCsvFilePath);

            Xeption actualException =
                await Assert.ThrowsAsync<Xeption>(
                    readCsvDataTask.AsTask);

            // Then
            Assert.Same(blpuException, actualException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import - BLPU Processing",
                    "Processing BLPU File",
                    $"Starting processing file {blpuCsvFilePath}.",
                    blpuCsvFilePath,
                    inputCorrelationId.ToString()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Starting processing file {blpuCsvFilePath}."),
                        Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapBLPUDataToAddressesAsync(
                    blpuCsvFilePath,
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

