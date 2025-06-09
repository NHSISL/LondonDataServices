// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Addresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnBulkAddAddressesIfInvalidAndLogItAsyncWithFolderPath(
            string invalidText)
        {
            // Given
            string invalidFolderPath = invalidText;

            var invalidArgumentAddressOrchestrationException =
                new InvalidArgumentAddressOrchestrationException(
                    message: "Invalid argument address orchestration exception, " +
                        "please correct the errors and try again.");

            invalidArgumentAddressOrchestrationException.AddData(
                key: "folderPath",
                values: "Text is required");

            var expectedAddressValidationOrchestrationException =
                new AddressValidationOrchestrationException(
                    message: "Address orchestration validation error occurred, please try again.",
                    innerException: invalidArgumentAddressOrchestrationException);

            // When
            ValueTask bulkAddAddressesTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(invalidFolderPath);

            AddressValidationOrchestrationException actualAddressValidationOrchestrationException =
                await Assert.ThrowsAsync<AddressValidationOrchestrationException>(
                    bulkAddAddressesTask.AsTask);

            // Then
            actualAddressValidationOrchestrationException.Should()
                .BeEquivalentTo(expectedAddressValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationOrchestrationException))),
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

