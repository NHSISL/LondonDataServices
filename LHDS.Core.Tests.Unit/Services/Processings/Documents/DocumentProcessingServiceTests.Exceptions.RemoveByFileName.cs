// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
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
            ShouldThrowDependencyValidationExceptionOnRemoveIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var randomContainer = GetRandomString();
            var randomString = GetRandomString();
            var randomBytes = Encoding.UTF8.GetBytes(GetRandomString());
            var randomMessage = GetRandomString();

            Document inputDocument = new Document
            {
                FileName = randomString,
                DocumentData = new MemoryStream(randomBytes)
            };

            var expectedDocumentProcessingDependencyValidationException =
                new DocumentProcessingDependencyValidationException(
                    message: "Document processing dependency validation occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.documentServiceMock.Setup(service =>
                service.RemoveDocumentByFileNameAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask retrieveDocumentTask =
                this.documentProcessingService.RemoveDocumentByFileNameAsync(
                    fileName: inputDocument.FileName, container: randomContainer);

            DocumentProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DocumentProcessingDependencyValidationException>(retrieveDocumentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDocumentProcessingDependencyValidationException);

            this.documentServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(It.IsAny<string>(), It.IsAny<string>()),
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
        public async Task ShouldThrowDependencyOnRemoveIfDependencyErrorOccursAndLogItAsync(
          Xeption dependencyException)
        {
            // given
            var randomContainer = GetRandomString();
            var randomString = GetRandomString();
            var randomBytes = Encoding.UTF8.GetBytes(GetRandomString());
            var randomMessage = GetRandomString();

            Document inputDocument = new Document
            {
                FileName = randomString,
                DocumentData = new MemoryStream(randomBytes)
            };

            var expectedDocumentProcessingDependencyException =
                new DocumentProcessingDependencyException(
                    message: "Document processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.documentServiceMock.Setup(service =>
                service.RemoveDocumentByFileNameAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask retrieveDocumentTask =
                this.documentProcessingService.RemoveDocumentByFileNameAsync(
                    fileName: inputDocument.FileName, container: randomContainer);

            DocumentProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DocumentProcessingDependencyException>(retrieveDocumentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDocumentProcessingDependencyException);

            this.documentServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDocumentProcessingDependencyException))),
                         Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAsync()
        {
            // given
            var randomContainer = GetRandomString();
            var randomString = GetRandomString();
            var randomBytes = Encoding.UTF8.GetBytes(GetRandomString());
            var randomMessage = GetRandomString();

            Document inputDocument = new Document
            {
                FileName = randomString,
                DocumentData = new MemoryStream(randomBytes)
            };

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
                service.RemoveDocumentByFileNameAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask retrieveDocumentTask =
                this.documentProcessingService.RemoveDocumentByFileNameAsync(
                    fileName: inputDocument.FileName, container: randomContainer);

            DocumentProcessingServiceException actualException =
                await Assert.ThrowsAsync<DocumentProcessingServiceException>(retrieveDocumentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDocumentProcessingServiveException);

            this.documentServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(It.IsAny<string>(), It.IsAny<string>()),
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
