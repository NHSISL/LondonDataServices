// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Documents
{
    public partial class DocumentProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationExceptionOnRetrieveIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string inputContainer = GetRandomString();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            Stream randomStream = new MemoryStream();
            Stream outputStream = randomStream;
            var randomMessage = GetRandomString();

            var expectedDocumentProcessingDependencyValidationException =
                new DocumentProcessingDependencyValidationException(
                    message: "Document processing dependency validation occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.documentServiceMock.Setup(service =>
                service.RetrieveDocumentByFileNameAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask retrieveDocumentTask =
                this.documentProcessingService.RetrieveDocumentByFileNameAsync(
                    output: outputStream,
                    fileName: inputFileName,
                    container: inputContainer);

            DocumentProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DocumentProcessingDependencyValidationException>(retrieveDocumentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDocumentProcessingDependencyValidationException);

            this.documentServiceMock.Verify(service =>
                service.RetrieveDocumentByFileNameAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDocumentProcessingDependencyValidationException))),
                         Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnRetrieveIfDependencyErrorOccursAndLogItAsync(
          Xeption dependencyException)
        {
            // given
            string inputContainer = GetRandomString();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            Stream randomStream = new MemoryStream();
            Stream outputStream = randomStream;
            var randomMessage = GetRandomString();

            var expectedDocumentProcessingDependencyException =
                new DocumentProcessingDependencyException(
                    message: "Document processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.documentServiceMock.Setup(service =>
                service.RetrieveDocumentByFileNameAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask retrieveDocumentTask =
                this.documentProcessingService.RetrieveDocumentByFileNameAsync(
                    output: outputStream,
                    fileName: inputFileName,
                    container: inputContainer);

            DocumentProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DocumentProcessingDependencyException>(retrieveDocumentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDocumentProcessingDependencyException);

            this.documentServiceMock.Verify(service =>
                service.RetrieveDocumentByFileNameAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDocumentProcessingDependencyException))),
                         Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveIfServiceErrorOccursAsync()
        {
            // given
            string inputContainer = GetRandomString();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            Stream randomStream = new MemoryStream();
            Stream outputStream = randomStream;
            var randomMessage = GetRandomString();
            var serviceException = new Exception();

            var failedDocumentProcessingServiceException =
                new FailedDocumentProcessingServiceException(
                    message: "Failed document processing service error occurred, please contact support.",
                    serviceException);

            var expectedDocumentProcessingServiveException =
                new DocumentProcessingServiceException(
                    message: "Document processing service error occurred, please contact support.",
                    failedDocumentProcessingServiceException);

            this.documentServiceMock.Setup(service =>
                service.RetrieveDocumentByFileNameAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask retrieveDocumentTask =
                this.documentProcessingService.RetrieveDocumentByFileNameAsync(
                    output: outputStream,
                    fileName: inputFileName,
                    container: inputContainer);

            DocumentProcessingServiceException actualException =
                await Assert.ThrowsAsync<DocumentProcessingServiceException>(retrieveDocumentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDocumentProcessingServiveException);

            this.documentServiceMock.Verify(service =>
                service.RetrieveDocumentByFileNameAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDocumentProcessingServiveException))),
                         Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
