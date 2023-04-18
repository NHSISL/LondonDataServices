// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnRetrieveOptOutIfOptOutFileIsNullAndLogItAsync(byte[] invalidData)
        {
            var randomString = GetRandomString();

            var invalidArgumentRetieveOptOutStatusOrchestrationException =
               new InvalidArgumentRetieveOptOutStatusOrchestrationException();

            invalidArgumentRetieveOptOutStatusOrchestrationException.AddData(
               key: "OptOutFile",
               values: "Data is required");

            var expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException =
               new RetrieveOptOutStatusOrchestrationValidationException(
                   innerException: invalidArgumentRetieveOptOutStatusOrchestrationException,
                   validationSummary: GetValidationSummary(invalidArgumentRetieveOptOutStatusOrchestrationException.Data));

            // when
            ValueTask RetrieveOptOutStatusTask =
                this.optOutOrchestrationService.RetrieveOptOutStatusAsync(invalidData, randomString);

            RetrieveOptOutStatusOrchestrationValidationException actualException =
               await Assert.ThrowsAsync<RetrieveOptOutStatusOrchestrationValidationException>(RetrieveOptOutStatusTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveOptOutIfRequestIdIsNullAndLogItAsync(string invalidText)
        {
            var randomString = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);

            var invalidArgumentRetieveOptOutStatusOrchestrationException =
               new InvalidArgumentRetieveOptOutStatusOrchestrationException();

            invalidArgumentRetieveOptOutStatusOrchestrationException.AddData(
               key: "RequestId",
               values: "Text is required");

            var expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException =
               new RetrieveOptOutStatusOrchestrationValidationException(
                   innerException: invalidArgumentRetieveOptOutStatusOrchestrationException,
                   validationSummary: GetValidationSummary(invalidArgumentRetieveOptOutStatusOrchestrationException.Data));

            // when
            ValueTask RetrieveOptOutStatusTask =
                this.optOutOrchestrationService.RetrieveOptOutStatusAsync(randomBytes, invalidText);

            RetrieveOptOutStatusOrchestrationValidationException actualException =
               await Assert.ThrowsAsync<RetrieveOptOutStatusOrchestrationValidationException>(RetrieveOptOutStatusTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRetrieveOptOutStatusOrchestrationOptOutFileValidationException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
