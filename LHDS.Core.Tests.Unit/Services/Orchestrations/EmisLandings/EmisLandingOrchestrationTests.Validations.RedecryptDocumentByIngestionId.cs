// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRedecryptIfIngestionIdIsNullAndLogItAsync()
        {
            // given
            Guid nullIngestionId = Guid.Empty;

            var invalidArgumentEmisLandingOrchestrationException =
                new InvalidArgumentEmisLandingOrchestrationException(
                    message: "Invalid EMIS landing orchestration argument(s), " +
                    "please correct the errors and try again.");

            invalidArgumentEmisLandingOrchestrationException.AddData(
                key: "ingestionTrackingId",
                values: "Id is required");

            var expectedEmisLandingOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentEmisLandingOrchestrationException);

            // when
            ValueTask redecryptTask = this.emisLandingOrchestrationService
                .RedecryptDocumentByIngestionIdAsync(ingestionTrackingId: nullIngestionId);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(redecryptTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
        }
    }
}
