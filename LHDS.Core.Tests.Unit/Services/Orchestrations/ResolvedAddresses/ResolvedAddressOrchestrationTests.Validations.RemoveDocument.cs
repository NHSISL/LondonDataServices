// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionsOnRemoveIfArgumentsIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string invalidFileName = invalidText;
            string invalidContainer = invalidText;

            var invalidArgumentResolvedAddressOrchestrationException =
                new InvalidArgumentResolvedAddressOrchestrationException(
                    message: $"Invalid resolved address orchestration argument.  " +
                        $"Please correct the errors and try again.");

            invalidArgumentResolvedAddressOrchestrationException.AddData(
                key: "fileName",
                values: "Text is required");

            invalidArgumentResolvedAddressOrchestrationException.AddData(
                key: "container",
                values: "Text is required");

            var expectedResolvedAddressOrchestrationValidationException =
                new ResolvedAddressOrchestrationValidationException(
                    message: "Resolved address validation errors occured, please try again.",
                    invalidArgumentResolvedAddressOrchestrationException);

            // when
            ValueTask addDocumentTask =
                this.resolvedAddressOrchestrationService.RemoveDocumentByFileNameAsync(
                    fileName: invalidFileName,
                    container: invalidContainer);

            ResolvedAddressOrchestrationValidationException actualResolvedAddressOrchestrationValidationException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationValidationException>(addDocumentTask.AsTask);

            //then
            actualResolvedAddressOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
