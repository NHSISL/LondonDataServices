// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveListOfDocumentsIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedDownloadProcessingDependencyValidationException =
                new DownloadProcessingDependencyValidationException(
                    message: "Download processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveListOfDocumentsToProcessAsync())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<List<Document>> downloadRetrieveListOfDocumentsTask =
                this.downloadProcessingService.RetrieveListOfDocumentsToProcessAsync();

            DownloadProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DownloadProcessingDependencyValidationException>(
                    downloadRetrieveListOfDocumentsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadProcessingDependencyValidationException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
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
            var expectedDownloadProcessingDependencyException =
                new DownloadProcessingDependencyException(
                    message: "Download processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveListOfDocumentsToProcessAsync())
                    .Throws(dependencyException);

            // when
            ValueTask<List<Document>> downloadRetrieveListOfDocumentsTask =
                this.downloadProcessingService.RetrieveListOfDocumentsToProcessAsync();

            DownloadProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DownloadProcessingDependencyException>(
                    downloadRetrieveListOfDocumentsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadProcessingDependencyException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDownloadProcessingDependencyException))),
                         Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveListOfDocumentsIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedDownloadProcessingServiceException =
                new FailedDownloadProcessingServiceException(
                    message: "Failed Download processing service error occurred, contact support.",
                    innerException: serviceException);

            var expectedDownloadProcessingServiveException =
                new DownloadProcessingServiceException(
                    message: "Download processing service error occurred, contact support.",
                    innerException: failedDownloadProcessingServiceException);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveListOfDocumentsToProcessAsync())
                    .Throws(serviceException);

            // when
            ValueTask<List<Document>> downloadRetrieveListOfDocumentsTask =
                this.downloadProcessingService.RetrieveListOfDocumentsToProcessAsync();

            DownloadProcessingServiceException actualException =
                await Assert.ThrowsAsync<DownloadProcessingServiceException>(
                    downloadRetrieveListOfDocumentsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadProcessingServiveException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDownloadProcessingServiveException))),
                         Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}