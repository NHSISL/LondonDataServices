// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Documents
{
    public partial class DocumentProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnGetDownloadLinkIfDocumentProcessingIsNullAndLogItAsync()
        {
            // given
            string nullFileName = null;

            Document document = new Document
            {
                FileName = nullFileName
            };

            var nullDocumentProcessingFileNameException =
                new NullDocumentProcessingFileNameException();

            var expectedDocumentProcessingValidationException =
                new DocumentProcessingValidationException(nullDocumentProcessingFileNameException);

            // when
            ValueTask<string> GetDownloadLinkTask =
                this.documentProcessingService.GetDownloadLinkAsync(document.FileName);

            DocumentProcessingValidationException actualDocumentProcessingValidationException =
                await Assert.ThrowsAsync<DocumentProcessingValidationException>(GetDownloadLinkTask.AsTask);

            //then
            actualDocumentProcessingValidationException.Should()
                .BeEquivalentTo(expectedDocumentProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDocumentProcessingValidationException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetDownloadLinkIfServiceErrorOccursAsync()
        {
            // given
            var randomString = GetRandomString();
            var randomBytes = Encoding.ASCII.GetBytes(GetRandomString());
            var randomMessage = GetRandomString();

            Document inputDocument = new Document
            {
                FileName = randomString,
                DocumentData = randomBytes
            };

            var serviceException = new Exception();

            var failedDocumentProcessingServiceException =
                new FailedDocumentProcessingServiceException(serviceException);

            var expectedDocumentProcessingServiveException =
                new DocumentProcessingServiceException(
                    failedDocumentProcessingServiceException);

            this.documentServiceMock.Setup(service =>
                service.GetDownloadLinkAsync(inputDocument.FileName))
                    .Throws(serviceException);

            // when
            ValueTask<string> retrieveDocumentTask =
                this.documentProcessingService.GetDownloadLinkAsync(inputDocument.FileName);

            DocumentProcessingServiceException actualException =
                await Assert.ThrowsAsync<DocumentProcessingServiceException>(retrieveDocumentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDocumentProcessingServiveException);

            this.documentServiceMock.Verify(service =>
                service.GetDownloadLinkAsync(inputDocument.FileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDocumentProcessingServiveException))),
                         Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
