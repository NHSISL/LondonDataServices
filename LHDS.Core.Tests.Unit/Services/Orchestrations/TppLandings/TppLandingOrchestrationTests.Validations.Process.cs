// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Orchestrations.TppLandings.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TppLandings
{
    public partial class TppLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfDocumentIsNullAndLogItAsync()
        {
            // given
            Document randonNullDocument = null;
            Guid randomSupplierId = Guid.NewGuid();

            var nullTppDocumentException =
                new NullDocumentTppLandingException(
                     message: "Document is Null");

            var expectepOrchestrationValidationException =
                new TppLandingOrchestrationValidationException(
                    message: "TPP landing orchestration validation errors occured, please try again.",
                    innerException: nullTppDocumentException);

            // when
            ValueTask<Guid> returnedGuidTask = this.tppOrchestrationService
                .ProcessAsync(document: randonNullDocument, supplierId: randomSupplierId);

            TppLandingOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<TppLandingOrchestrationValidationException>(returnedGuidTask.AsTask);

            // then
            actualException.Should()
               .BeEquivalentTo(expectepOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectepOrchestrationValidationException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionIfDocumentFileNameIsNullAndLogItAsync(string invalidText)
        {
            // given
            Guid supplierId = Guid.Empty;
            Document randomDocument = CreateRandomDocument();
            randomDocument.FileName = invalidText;

            var invalidArgumentException =
                new InvalidArgumentTppLandingOrchestrationException(
                    message: "Invalid TPP landing orchestration argument(s), please correct the errors and try again.");

            invalidArgumentException.AddData(
               key: "FileName",
               values: "Text is required");

            invalidArgumentException.AddData(
               key: "SupplierId",
               values: "Id is required");

            var expectedTppOrchestrationValidationException =
                new TppLandingOrchestrationValidationException(
                    message: "TPP landing orchestration validation errors occured, please try again.",
                    innerException: invalidArgumentException);

            // when
            ValueTask<Guid> returnedGuidTask = this.tppOrchestrationService
                .ProcessAsync(document: randomDocument, supplierId);

            TppLandingOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<TppLandingOrchestrationValidationException>(returnedGuidTask.AsTask);

            // then
            actualException.Should()
               .BeEquivalentTo(expectedTppOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTppOrchestrationValidationException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}