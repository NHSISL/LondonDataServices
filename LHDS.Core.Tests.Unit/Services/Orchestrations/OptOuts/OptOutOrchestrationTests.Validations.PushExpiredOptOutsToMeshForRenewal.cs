// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfConfigurationSectionIsNullAndLogItAsync()
        {
            // Given
            var invalidConfigOptOutOrchestrationException =
              new InvalidConfigOptOutOrchestrationException();

            invalidConfigOptOutOrchestrationException.AddData(
                key: nameof(OptOutConfiguration.OutputFolder),
                values: "Text is required");

            invalidConfigOptOutOrchestrationException.AddData(
               key: nameof(OptOutConfiguration.ExpiredAfterDays),
               values: "Value is required");

            var expectedPushExpiredOptOutsToMeshIfExpiredOrchestrationOptOutFileValidationException =
              new OptOutOrchestrationValidationException(
                  innerException: invalidConfigOptOutOrchestrationException,
                  validationSummary: GetValidationSummary(invalidConfigOptOutOrchestrationException.Data));

            // When
            ValueTask pushExpOptOutsToMeshIfExpiredTask =
               this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            OptOutOrchestrationValidationException actualException =
              await Assert.ThrowsAsync<OptOutOrchestrationValidationException>(pushExpOptOutsToMeshIfExpiredTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedPushExpiredOptOutsToMeshIfExpiredOrchestrationOptOutFileValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPushExpiredOptOutsToMeshIfExpiredOrchestrationOptOutFileValidationException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
