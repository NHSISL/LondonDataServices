// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DownloadDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnProcessIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var expectedDependencyException =
                new DownloadOrchestrationDependencyValidationException(
                    message: "Download orchestration dependency validation error occurred, fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
              service.RetrieveListOfDocumentsToProcessAsync())
                  .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<List<string>> processTask = this.downloadOrchestrationService.ProcessAsync();

            DownloadOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationDependencyValidationException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadServiceMock.Verify(service =>
              service.RetrieveListOfDocumentsToProcessAsync(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DownloadDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            var expectedDependencyException =
                new DownloadOrchestrationDependencyException(
                    message: "Download orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
              service.RetrieveListOfDocumentsToProcessAsync())
                  .ThrowsAsync(dependancyException);

            // when
            ValueTask<List<string>> processTask = this.downloadOrchestrationService.ProcessAsync();

            DownloadOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationDependencyException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadServiceMock.Verify(service =>
              service.RetrieveListOfDocumentsToProcessAsync(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            var serviceException = new Exception();

            var failedDownloadOrchestrationServiceException =
                new FailedDownloadOrchestrationServiceException(
                    message: "Failed download orchestration service occurred, please contact support",
                    serviceException);

            var expectedDownloadOrchestrationServiceException =
                new DownloadOrchestrationServiceException(
                    message: "Download orchestration service error occurred, contact support.",
                    failedDownloadOrchestrationServiceException);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveListOfDocumentsToProcessAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<string>> processTask = this.downloadOrchestrationService.ProcessAsync();

            DownloadOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadOrchestrationServiceException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadOrchestrationServiceException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DownloadDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnProcessFileIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var fileName = GetRandomMessage();

            var expectedDependencyException =
                new DownloadOrchestrationDependencyValidationException(
                    message: "Download orchestration dependency validation error occurred, fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(fileName))
                    .Throws(dependancyValidationException);

            // when
            ValueTask<string> processTask = this.downloadOrchestrationService.ProcessAsync(fileName);

            DownloadOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationDependencyValidationException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(fileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DownloadDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessFileIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            var fileName = GetRandomMessage();

            var expectedDependencyException =
                new DownloadOrchestrationDependencyException(
                    message: "Download orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(fileName))
                    .Throws(dependancyException);

            // when
            ValueTask<string> processTask = this.downloadOrchestrationService.ProcessAsync(fileName);

            DownloadOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationDependencyException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(fileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessFileIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            var fileName = GetRandomMessage();

            var serviceException = new Exception();

            var failedDownloadOrchestrationServiceException =
                new FailedDownloadOrchestrationServiceException(
                    message: "Failed download orchestration service occurred, please contact support",
                    serviceException);

            var expectedDownloadOrchestrationServiceException =
                new DownloadOrchestrationServiceException(
                    message: "Download orchestration service error occurred, contact support.",
                    failedDownloadOrchestrationServiceException);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(fileName))
                    .Throws(serviceException);

            // when
            ValueTask<string> processTask = this.downloadOrchestrationService.ProcessAsync(fileName);

            DownloadOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadOrchestrationServiceException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(fileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadOrchestrationServiceException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
