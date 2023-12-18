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
               new TppDocumentValidationException(
                   message: "Tpp Document validation errors occured, please try again",
                   innerException: nullDocumentException);

            // when
            ValueTask<Guid> returnedGuidTask = this.tppOrchestrationService.ProcessAsync(randonNullDocument);

            TppDocumentValidationException actualException =
               await Assert.ThrowsAsync<TppDocumentValidationException>(returnedGuidTask.AsTask);

            // then
            actualException.Should()
               .BeEquivalentTo(expectedDocumentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDocumentValidationException))),
                        Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }
    }
}