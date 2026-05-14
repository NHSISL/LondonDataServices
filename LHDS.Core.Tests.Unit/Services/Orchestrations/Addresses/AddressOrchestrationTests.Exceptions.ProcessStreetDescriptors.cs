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
        public async Task ShouldThrowAggregateExceptionOnProcessStreetDescriptorsIfErrorsInMapCallsAsync()
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

            string streetDescriptorsCsvFilePath = "ID15.csv";
            Guid randomGuid = Guid.NewGuid();
            Guid inputCorrelationId = randomGuid;
            Xeption streetDescriptorsException = new Xeption();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(inputCorrelationId);

            addressOrchestrationServiceMock.Setup(service =>
                service.MapStreetDescriptorDataToAddressesAsync(
                    streetDescriptorsCsvFilePath,
                    default))
                        .Throws(streetDescriptorsException);

            AddressOrchestrationService service =
                addressOrchestrationServiceMock.Object;

            // When
            ValueTask readCsvDataTask =
                service.ProcessStreetDescriptorDataAsync(
                    streetDescriptorsCsvFilePath);

            Xeption actualException =
                await Assert.ThrowsAsync<Xeption>(
                    readCsvDataTask.AsTask);

            // Then
            Assert.Same(
                streetDescriptorsException, actualException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import - Street Descriptors Processing",
                    "Processing Street Descriptors File",
                    $"Starting processing file " +
                        $"{streetDescriptorsCsvFilePath}.",
                    streetDescriptorsCsvFilePath,
                    inputCorrelationId.ToString()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Starting processing file " +
                        $"{streetDescriptorsCsvFilePath}."),
                        Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapStreetDescriptorDataToAddressesAsync(
                    streetDescriptorsCsvFilePath,
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

