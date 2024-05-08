// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text;
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
        [InlineData(null)]
        [InlineData(new byte[] { })]
        public async Task ShouldThrowValidationExceptionOnRetrieveOptOutIfOptOutFileIsNullAndLogItAsync(
            byte[] invalidData)
        {
            var randomString = GetRandomString();

            var invalidArgumentRetieveOptOutStatusOrchestrationException =
                new InvalidArgumentOptOutOrchestrationException(
                    message: "Invalid Retrieve Opt Out Status orchestration argument(s), " +
                        "please correct the errors and try again.");

            invalidArgumentRetieveOptOutStatusOrchestrationException.AddData(
               key: "OptOutFile",
               values: "Data is required");

            var expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException =
                new OptOutOrchestrationValidationException(
                    message: "Opt Out orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentRetieveOptOutStatusOrchestrationException);

            // when
            ValueTask<string> RetrieveOptOutStatusTask =
                this.optOutOrchestrationService.RetrieveOptOutStatusAsync(invalidData, randomString);

            OptOutOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationValidationException>(RetrieveOptOutStatusTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveOptOutIfRequestIdIsNullAndLogItAsync(
            string invalidText)
        {
            var randomString = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);

            var invalidArgumentRetieveOptOutStatusOrchestrationException =
                new InvalidArgumentOptOutOrchestrationException(
                    message: "Invalid Retrieve Opt Out Status orchestration argument(s), " +
                        "please correct the errors and try again.");

            invalidArgumentRetieveOptOutStatusOrchestrationException.AddData(
               key: "RequestId",
               values: "Text is required");

            var expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException =
                new OptOutOrchestrationValidationException(
                    message: "Opt Out orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentRetieveOptOutStatusOrchestrationException);

            // when
            ValueTask<string> RetrieveOptOutStatusTask =
                this.optOutOrchestrationService.RetrieveOptOutStatusAsync(randomBytes, invalidText);

            OptOutOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationValidationException>(RetrieveOptOutStatusTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
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
