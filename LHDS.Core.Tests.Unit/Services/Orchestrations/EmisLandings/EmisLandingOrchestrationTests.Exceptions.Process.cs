// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DownloadDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnProcessIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var expectedDependencyException =
                new EmisLandingOrchestrationDependencyValidationException(
                    message: "EMIS landing orchestration dependency validation error occurred, fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.downloadProcessingServiceMock.Setup(service =>
              service.RetrieveListOfDocumentsToProcessAsync())
                  .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<List<string>> processTask = this.emisLandingOrchestrationService.ProcessAsync();

            EmisLandingOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationDependencyValidationException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadProcessingServiceMock.Verify(service =>
              service.RetrieveListOfDocumentsToProcessAsync(),
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

        [Theory]
        [MemberData(nameof(DownloadDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            var expectedDependencyException =
                new EmisLandingOrchestrationDependencyException(
                    message: "EMIS landing orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.downloadProcessingServiceMock.Setup(service =>
              service.RetrieveListOfDocumentsToProcessAsync())
                  .ThrowsAsync(dependancyException);

            // when
            ValueTask<List<string>> processTask = this.emisLandingOrchestrationService.ProcessAsync();

            EmisLandingOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationDependencyException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadProcessingServiceMock.Verify(service =>
              service.RetrieveListOfDocumentsToProcessAsync(),
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

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            var serviceException = new Exception();

            var failedEmisLandingOrchestrationServiceException =
                new FailedEmisLandingOrchestrationServiceException(
                    message: "Failed EMIS landing orchestration service occurred, please contact support",
                    serviceException);

            var expectedEmisLandingOrchestrationServiceException =
                new EmisLandingOrchestrationServiceException(
                    message: "EMIS landing orchestration service error occurred, contact support.",
                    failedEmisLandingOrchestrationServiceException);

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveListOfDocumentsToProcessAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<string>> processTask = this.emisLandingOrchestrationService.ProcessAsync();

            EmisLandingOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedEmisLandingOrchestrationServiceException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationServiceException))),
                        Times.Once);

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DownloadDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnProcessFileIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var fileName = GetRandomMessage();

            var expectedDependencyException =
                new EmisLandingOrchestrationDependencyValidationException(
                    message: "EMIS landing orchestration dependency validation error occurred, fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(fileName))
                    .Throws(dependancyValidationException);

            // when
            ValueTask<string> processTask = this.emisLandingOrchestrationService.ProcessAsync(fileName);

            EmisLandingOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationDependencyValidationException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(fileName),
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

        [Theory]
        [MemberData(nameof(DownloadDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessFileIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            var fileName = GetRandomMessage();

            var expectedDependencyException =
                new EmisLandingOrchestrationDependencyException(
                    message: "EMIS landing orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(fileName))
                    .Throws(dependancyException);

            // when
            ValueTask<string> processTask = this.emisLandingOrchestrationService.ProcessAsync(fileName);

            EmisLandingOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationDependencyException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(fileName),
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

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessFileIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            var fileName = GetRandomMessage();
            var serviceException = new Exception();

            var failedEmisLandingOrchestrationServiceException =
                new FailedEmisLandingOrchestrationServiceException(
                    message: "Failed EMIS landing orchestration service occurred, please contact support",
                    serviceException);

            var expectedEmisLandingOrchestrationServiceException =
                new EmisLandingOrchestrationServiceException(
                    message: "EMIS landing orchestration service error occurred, contact support.",
                    failedEmisLandingOrchestrationServiceException);

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(fileName))
                    .Throws(serviceException);

            // when
            ValueTask<string> processTask = this.emisLandingOrchestrationService.ProcessAsync(fileName);

            EmisLandingOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedEmisLandingOrchestrationServiceException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(fileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationServiceException))),
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
