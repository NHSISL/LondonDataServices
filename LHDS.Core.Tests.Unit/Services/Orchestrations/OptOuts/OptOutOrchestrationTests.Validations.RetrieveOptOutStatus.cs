// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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

            var expectedRetrieveOptOutStatusOrchestrationFileNameValidationException =
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
                .BeEquivalentTo(expectedRetrieveOptOutStatusOrchestrationFileNameValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRetrieveOptOutStatusOrchestrationFileNameValidationException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
