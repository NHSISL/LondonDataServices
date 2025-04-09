// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Addresses.Exceptions;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnReadCsvDataAndBulkAddAddressesFilesInvalidAsync()
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

            string inputTempPath = GetRandomString();
            string inputSearchPattern = "*.csv";
            List<string> fileList = ["ID15.csv", "ID21.csv", "ID24.csv", "ID28.csv"];
            Random random = new Random();
            int randomIndex = random.Next(0, fileList.Count);
            List<string> invalidFileList = fileList;
            invalidFileList.RemoveAt(randomIndex);

            var invalidFileAddressOrchestrationException =
                new InvalidFileAddressOrchestrationException(
                    message: $"The zip file does not contain the required csv files. " +
                        $"Please correct the errors and try again.");

            InvalidFileAddressOrchestrationException expectedFileAddressOrchestrationException =
                invalidFileAddressOrchestrationException;

            this.fileBrokerMock.Setup(broker =>
                broker.GetListOfFilesAsync(inputTempPath, inputSearchPattern))
                    .ReturnsAsync(invalidFileList);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            ValueTask loadAndMapCsvTask = service.ReadCsvDataAndBulkAddAddressesAsync(inputTempPath);

            InvalidFileAddressOrchestrationException actualFileAddressOrchestrationException =
                await Assert.ThrowsAsync<InvalidFileAddressOrchestrationException>(
                    loadAndMapCsvTask.AsTask);

            // Then
            actualFileAddressOrchestrationException.Should()
                .BeEquivalentTo(expectedFileAddressOrchestrationException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFilesAsync(inputTempPath, inputSearchPattern),
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

