// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Tpp.Exceptions;
using LHDS.Core.Models.Orchestrations.Tpp.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Tpp
{
    public partial class TppOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfDocumentIsNullAndLogItAsync()
        {
            // given
            Models.Foundations.Documents.Document randonNullDocument = null;

            var nullDocumentException =
                new NullTppDocumentException(
                     message: "Document is Null");

            var expectedDocumentValidationException =
                new TppOrchestrationValidationException(
                    message: "TPP Orchestration validation errors occured, please try again.",
                    innerException: nullDocumentException);

            // when
            ValueTask<Guid> returnedGuidTask = this.tppOrchestrationService.ProcessAsync(randonNullDocument);

            TppOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<TppOrchestrationValidationException>(returnedGuidTask.AsTask);

            // then
            actualException.Should()
               .BeEquivalentTo(expectedDocumentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDocumentValidationException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionIfDocumentFileNameIsNullAndLogItAsync(string invalidText)
        {
            // given
            Models.Foundations.Documents.Document randomDocument = CreateRandomDocument();
            randomDocument.FileName = invalidText;

            var invalidArgumentException =
                new InvalidArgumentTppOrchestrationException(
                    message: "Invalid TPP orchestration argument(s), please correct the errors and try again.");

            invalidArgumentException.AddData(
               key: "FileName",
               values: "Text is required");

            var expectedTppOrchestrationValidationException =
                new TppOrchestrationValidationException(
                    message: "TPP Orchestration validation errors occured, please try again.",
                    innerException: invalidArgumentException);

            // when
            ValueTask<Guid> returnedGuidTask = this.tppOrchestrationService.ProcessAsync(randomDocument);

            TppOrchestrationValidationException actualException =
               await Assert.ThrowsAsync<TppOrchestrationValidationException>(returnedGuidTask.AsTask);

            // then
            actualException.Should()
               .BeEquivalentTo(expectedTppOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTppOrchestrationValidationException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }
    }
}