// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
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
        [InlineData(null, null)]
        [InlineData("", "empty")]
        [InlineData(" ", "empty")]
        public async Task ShouldThrowValidationExceptionOnBulkAddAddressesIfInvalidAndLogItAsync(
            string invalidText,
            string invalidData)
        {
            // Given
            string invalidFileName = invalidText;

            Stream invalidStream = invalidData == null
                ? null
                : new MemoryStream();

            var invalidArgumentAddressOrchestrationException =
                new InvalidArgumentAddressOrchestrationException(
                    message: "Invalid argument address orchestration exception, " +
                        "please correct the errors and try again.");

            invalidArgumentAddressOrchestrationException.AddData(
                key: "input",
                values: "Stream is required");

            invalidArgumentAddressOrchestrationException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedAddressValidationOrchestrationException =
                new AddressValidationOrchestrationException(
                    message: "Address orchestration validation error occurred, please try again.",
                    innerException: invalidArgumentAddressOrchestrationException);

            // When
            ValueTask bulkAddAddressesTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(input: invalidStream, fileName: invalidFileName);

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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnBulkAddAddressesIfFileInvalidAndLogItAsync()
        {
            // Given
            string inputFileName = GetRandomString();
            string someStringData = GetRandomString();
            byte[] someData = Encoding.ASCII.GetBytes(someStringData);
            Stream inputStream = new MemoryStream(someData);
            string someMessage = GetRandomString();

            var invalidFileAddressOrchestrationException =
                new InvalidFileAddressOrchestrationException(
                    message: someMessage);

            var expectedAddressValidationOrchestrationException =
                new AddressValidationOrchestrationException(
                    message: "Address orchestration validation error occurred, please try again.",
                    innerException: invalidFileAddressOrchestrationException);

            this.fileBrokerMock.Setup(broker =>
                broker.GetTempPath())
                    .ThrowsAsync(invalidFileAddressOrchestrationException);

            // When
            ValueTask bulkAddAddressesTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(input: inputStream, fileName: inputFileName);

            AddressValidationOrchestrationException actualAddressValidationOrchestrationException =
                await Assert.ThrowsAsync<AddressValidationOrchestrationException>(
                    bulkAddAddressesTask.AsTask);

            // Then
            actualAddressValidationOrchestrationException.Should()
                .BeEquivalentTo(expectedAddressValidationOrchestrationException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetTempPath(),
                    Times.Once);

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

