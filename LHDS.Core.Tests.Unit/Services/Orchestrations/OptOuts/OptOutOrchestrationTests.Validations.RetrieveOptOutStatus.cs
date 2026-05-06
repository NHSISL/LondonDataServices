// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Theory]
        [InlineData(null, "null")]
        [InlineData("", "empty")]
        [InlineData(" ", "")]
        public async Task ShouldThrowValidationExceptionOnRetrieveOptOutIfArgumentsIsIbvalidAndLogItAsync(
            string invalidText, string streamType)
        {
            Stream invalidStream = streamType switch
            {
                "null" => null,
                _ => new MemoryStream()
            };

            var invalidArgumentRetieveOptOutStatusOrchestrationException =
                new InvalidArgumentOptOutOrchestrationException(
                    message: "Invalid Retrieve Opt Out Status orchestration argument(s), " +
                        "please correct the errors and try again.");

            invalidArgumentRetieveOptOutStatusOrchestrationException.AddData(
               key: "Input",
               values: "Stream is required");

            invalidArgumentRetieveOptOutStatusOrchestrationException.AddData(
               key: "FileName",
               values: "Text is required");

            var expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException =
                new OptOutOrchestrationValidationException(
                    message: "Opt Out orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentRetieveOptOutStatusOrchestrationException);

            // when
            ValueTask<string> RetrieveOptOutStatusTask =
                this.optOutOrchestrationService.RetrieveOptOutStatusAsync(
                    invalidStream, invalidText, TestContext.Current.CancellationToken);

            OptOutOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationValidationException>(RetrieveOptOutStatusTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
