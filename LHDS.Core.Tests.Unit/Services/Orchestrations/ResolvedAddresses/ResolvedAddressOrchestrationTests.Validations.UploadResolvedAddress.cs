// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
                    message: "Address extraction orchestration validation error occurred, please try again.",
                    innerException: invalidArgumentResolvedAddressOrchestrationException);

            // When
            ValueTask uploadTask = this.resolvedAddressOrchestrationService
                .UploadAddressesToReslveAsync(input: invalidStream, fileName: invalidFileName);

            ResolvedAddressOrchestrationValidationException actualResolvedAddressOrchestrationValidationException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationValidationException>(
                    uploadTask.AsTask);

            // Then
            actualResolvedAddressOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationValidationException);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<ResolvedAddress>(It.IsAny<string>(), true, fieldMappings),
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
        }
    }
}
