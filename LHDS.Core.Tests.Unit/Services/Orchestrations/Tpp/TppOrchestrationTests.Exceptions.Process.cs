// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Tpp.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Tpp
{
    public partial class TppOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(TppDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnProcessIfDependencyValidationOccursAndLogItAsync(
             Xeption dependancyValidationException)
        {
            Models.Foundations.Documents.Document randomDocument = CreateRandomDocument();

            // given
            var expectedDependencyException =
                new TppOrchestrationDependencyValidationException(
                    message: "Tpp Orchestration dependency validation error occurred, fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
              service.RetrieveAllIngestionTrackings())
                  .Throws(dependancyValidationException);

            // when
            ValueTask<Guid> processTask = this.tppOrchestrationService.ProcessAsync(randomDocument);

            TppOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TppOrchestrationDependencyValidationException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
              service.RetrieveAllIngestionTrackings(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }
    }
}