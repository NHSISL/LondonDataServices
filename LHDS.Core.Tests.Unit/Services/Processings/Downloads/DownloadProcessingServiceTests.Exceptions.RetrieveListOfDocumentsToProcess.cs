// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Processings.Downloads.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Downloads
{
    public partial class DownloadProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveListOfDocumentsIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };

            var expectedDownloadProcessingDependencyValidationException =
                new DownloadProcessingDependencyValidationException(
                    message: "Download processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveListOfDocumentsToProcessAsync(inputDownload))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<string>> downloadRetrieveListOfDocumentsTask =
                this.downloadProcessingService.RetrieveListOfDownloadsToProcessAsync(inputDownload);

            DownloadProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DownloadProcessingDependencyValidationException>(
                    downloadRetrieveListOfDocumentsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadProcessingDependencyValidationException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(inputDownload),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDownloadProcessingDependencyValidationException))),
                         Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveListOfDocumentsIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };

            var expectedDownloadProcessingDependencyException =
                new DownloadProcessingDependencyException(
                    message: "Download processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveListOfDocumentsToProcessAsync(inputDownload))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<string>> downloadRetrieveListOfDocumentsTask =
                this.downloadProcessingService.RetrieveListOfDownloadsToProcessAsync(inputDownload);

            DownloadProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DownloadProcessingDependencyException>(
                    downloadRetrieveListOfDocumentsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadProcessingDependencyException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(inputDownload),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDownloadProcessingDependencyException))),
                         Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveListOfDocumentsIfServiceErrorOccursAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            var serviceException = new Exception();

            var failedDownloadProcessingServiceException =
                new FailedDownloadProcessingServiceException(
                    message: "Failed Download processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDownloadProcessingServiveException =
                new DownloadProcessingServiceException(
                    message: "Download processing service error occurred, please contact support.",
                    innerException: failedDownloadProcessingServiceException);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveListOfDocumentsToProcessAsync(inputDownload))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<string>> downloadRetrieveListOfDocumentsTask =
                this.downloadProcessingService.RetrieveListOfDownloadsToProcessAsync(inputDownload);

            DownloadProcessingServiceException actualException =
                await Assert.ThrowsAsync<DownloadProcessingServiceException>(
                    downloadRetrieveListOfDocumentsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadProcessingServiveException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(inputDownload),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDownloadProcessingServiveException))),
                         Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}