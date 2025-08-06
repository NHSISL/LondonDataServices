// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessDecryptedItemsForBatchCompleteAsyncIfSupplierIdIsInvalidAsync()
        {
            // given
            Guid invalidSupplierId = default;

            var invalidArgumentIngresOrchestrationException =
                new InvalidArgumentIngressOrchestrationException(
                    message: "Invalid ingress orchestration argument(s), " +
                    "please correct the errors and try again.");

            invalidArgumentIngresOrchestrationException.AddData(
                key: "supplierId",
                values: "Id is required");

            var expectedIngresOrchestrationValidationException =
                new IngressOrchestrationValidationException(
                    message: "Ingress orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentIngresOrchestrationException);

            // when
            ValueTask rollbackIngestionTrackingItemTask = this.ingressOrchestrationService
                .ProcessDecryptedItemsForBatchCompleteAsync(invalidSupplierId);

            IngressOrchestrationValidationException actualIngressOrchestrationValidationException =
                await Assert.ThrowsAsync<IngressOrchestrationValidationException>(
                    rollbackIngestionTrackingItemTask.AsTask);

            // then
            actualIngressOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedIngresOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngresOrchestrationValidationException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
