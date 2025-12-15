// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Theory]
        [InlineData(null, "null")]
        [InlineData("", "empty")]
        [InlineData(" ", "empty")]
        public async Task ShouldThrowValidationExceptionOnUploadResolvedAddressIfInvalidAndLogItAsync(
            string invalidText,
            string streamType)
        {
            // Given
            string invalidFileName = invalidText;
            Stream invalidStream = streamType == "null" ? null : new MemoryStream();

            Dictionary<string, int> fieldMappings =
                new Dictionary<string, int>
                {
                    { nameof(ResolvedAddress.UniqueReference), 0 },
                    { nameof(ResolvedAddress.UnstructuredPostalAddress), 2 }
                };

            var invalidArgumentResolvedAddressOrchestrationException =
                new InvalidArgumentResolvedAddressOrchestrationException(
                    message: "Invalid argument resolved address orchestration exception, " +
                        "please correct the errors and try again.");

            invalidArgumentResolvedAddressOrchestrationException.AddData(
                key: "input",
                values: "Stream is required");

            invalidArgumentResolvedAddressOrchestrationException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedResolvedAddressOrchestrationValidationException =
                new ResolvedAddressOrchestrationValidationException(
                    message: "Resolved address validation errors occured, please try again.",
                    innerException: invalidArgumentResolvedAddressOrchestrationException);

            // When
            ValueTask uploadTask = this.resolvedAddressOrchestrationService
                .UploadAddressesToResolveAsync(input: invalidStream, fileName: invalidFileName);

            ResolvedAddressOrchestrationValidationException actualResolvedAddressOrchestrationValidationException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationValidationException>(
                    uploadTask.AsTask);

            // Then
            actualResolvedAddressOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressOrchestrationValidationException))),
                        Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<ResolvedAddress>(It.IsAny<string>(), true, fieldMappings, true),
                    Times.Never);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.BulkAddResolvedAddressesAsync(It.IsAny<List<ResolvedAddress>>(), It.IsAny<string>()),
                    Times.Never);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        public async Task ShouldThrowValidationExceptionOnNullUnstructuredPostalAddressIfInvalidAndLogItAsync(
            string nullText)
        {
            // Given
            string validFileName = "valid.csv";
            byte[] dummyData = new byte[] { 1, 2, 3 };
            Stream validStream = new MemoryStream(dummyData);

            Dictionary<string, int> fieldMappings =
                new Dictionary<string, int>
                {
                    { nameof(ResolvedAddress.UniqueReference), 0 },
                    { nameof(ResolvedAddress.UnstructuredPostalAddress), 2 }
                };

            var invalidArgumentResolvedAddressOrchestrationException =
                new InvalidArgumentResolvedAddressOrchestrationException(
                    message: "Invalid argument resolved address orchestration exception, " +
                        "please correct the errors and try again.");

            invalidArgumentResolvedAddressOrchestrationException.AddData(
                key: "UnstructuredPostalAddress",
                values: "Text is required");

            var expectedResolvedAddressOrchestrationValidationException =
                new ResolvedAddressOrchestrationValidationException(
                    message: "Resolved address validation errors occured, please try again.",
                    innerException: invalidArgumentResolvedAddressOrchestrationException);

            // Mock the CSV broker to return a ResolvedAddress with invalid UnstructuredPostalAddress
            var resolvedAddresses = new List<ResolvedAddress>
            {
                new ResolvedAddress
                {
                    UniqueReference = Guid.NewGuid(),
                    UnstructuredPostalAddress = nullText
                }
            };

            this.csvHelperBrokerMock
                .Setup(broker => broker.MapCsvToObjectAsync<ResolvedAddress>(
                    It.IsAny<string>(), true, fieldMappings, true))
                .ReturnsAsync(resolvedAddresses);

            // When
            ValueTask uploadTask = this.resolvedAddressOrchestrationService
                .UploadAddressesToResolveAsync(input: validStream, fileName: validFileName);

            ResolvedAddressOrchestrationValidationException actualResolvedAddressOrchestrationValidationException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationValidationException>(
                    uploadTask.AsTask);

            // Then
            actualResolvedAddressOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressOrchestrationValidationException))),
                        Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<ResolvedAddress>(It.IsAny<string>(), true, fieldMappings, true),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.BulkAddResolvedAddressesAsync(It.IsAny<List<ResolvedAddress>>(), It.IsAny<string>()),
                    Times.Never);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.auditBrokerMock.Verify(broker =>
            broker.LogAsync(
                "Resolved Address Upload",
                "Uploading Resolved Addresses",
                It.IsAny<string>(),
                null,
                It.IsAny<string>(),
                "Information"),
            Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}
