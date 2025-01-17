// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Processings.Downloads.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Downloads
{
    public partial class DownloadProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveByFileNameIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Download someDownload = new Download();

            var expectedDownloadProcessingDependencyValidationException =
                new DownloadProcessingDependencyValidationException(
                    message: "Download processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(It.IsAny<Download>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask downloadRetrieveByFileNameTask =
                this.downloadProcessingService.RetrieveDownloadByFileNameAsync(someDownload);

            DownloadProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DownloadProcessingDependencyValidationException>(
                    downloadRetrieveByFileNameTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadProcessingDependencyValidationException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(It.IsAny<Download>()),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByFileNameIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Download someDownload = new Download();

            var expectedDownloadProcessingDependencyException =
                new DownloadProcessingDependencyException(
                    message: "Download processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(It.IsAny<Download>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask downloadRetrieveByFileNameTask =
                this.downloadProcessingService.RetrieveDownloadByFileNameAsync(someDownload);

            DownloadProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DownloadProcessingDependencyException>(downloadRetrieveByFileNameTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadProcessingDependencyException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(It.IsAny<Download>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDownloadProcessingDependencyException))),
                         Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByFileNameIfServiceErrorOccursAsync()
        {
            // given
            Download someDownload = new Download();

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
                service.RetrieveDownloadByFileNameAsync(It.IsAny<Download>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask downloadRetrieveByFileNameTask =
                this.downloadProcessingService.RetrieveDownloadByFileNameAsync(someDownload);

            DownloadProcessingServiceException actualException =
                await Assert.ThrowsAsync<DownloadProcessingServiceException>(downloadRetrieveByFileNameTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadProcessingServiveException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(It.IsAny<Download>()),
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