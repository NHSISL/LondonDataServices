// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRollBackIngestionTrackingItemIfPathIsInvalidAsync(
            string invalidText)
        {
            // given
            string enctyptedFileName = invalidText;

            var invalidArgumentIngresOrchestrationException =
                new InvalidArgumentIngressOrchestrationException(
                    message: "Invalid ingress orchestration argument(s), " +
                    "please correct the errors and try again.");

            invalidArgumentIngresOrchestrationException.AddData(
                key: nameof(IngestionTracking.EncryptedFileName),
                values: "Text is required");

            var expectedIngresOrchestrationValidationException =
                new IngressOrchestrationValidationException(
                    message: "Ingress orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentIngresOrchestrationException);

            // when
            ValueTask rollbackIngestionTrackingItemTask = this.ingressOrchestrationService
                .RollbackIngestionTrackingItemAsync(enctyptedFileName);

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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRollBackIngestionTrackingItemIfStorageIsNullAndLogItAsync()
        {
            // given
            string randomEncryptedFileName = GetRandomString();

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(null as IQueryable<IngestionTracking>);

            var notFoundIngressOrchestrationException =
                new NotFoundIngressOrchestrationException(
                    message: $"Couldn't find ingestion tracking with {nameof(IngestionTracking.EncryptedFileName)}: " +
                        $"{randomEncryptedFileName}.");

            var expectedIngresOrchestrationValidationException =
                new IngressOrchestrationValidationException(
                    message: "Ingress orchestration validation errors occurred, please try again.",
                    innerException: notFoundIngressOrchestrationException);

            // when
            ValueTask rollbackIngestionTrackingTask = this.ingressOrchestrationService
                .RollbackIngestionTrackingItemAsync(randomEncryptedFileName);

            IngressOrchestrationValidationException actualIngressOrchestrationValidationException =
                await Assert.ThrowsAsync<IngressOrchestrationValidationException>(
                    rollbackIngestionTrackingTask.AsTask);

            // then
            actualIngressOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedIngresOrchestrationValidationException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

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
