// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using Azure;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Documents
{
    public partial class DocumentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteFileAndLogItAsync()
        {
            // given
            string randomFileName = GetRandomString();
            var randomMessage = GetRandomString();
            var isDecrypted = false;

            var blobContainerName = this.inMemoryConfiguration
                .GetValue<string>("blobStorage:encryptedBlobContainerName");

            Document randomDocument = new Document
            {
                FileName = randomFileName,
                DocumentData = Encoding.ASCII.GetBytes(GetRandomString())
            };

            var requestFailedException = new RequestFailedException(randomMessage);

            var failedDocumentRequestException =
                new FailedDocumentRequestException(requestFailedException);

            var expectedDependencyException =
                 new DocumentDependencyException(failedDocumentRequestException);

            this.blobStorageBrokerMock.Setup(broker =>
                 broker.DeleteFileAsync(randomDocument.FileName, blobContainerName))
                    .Throws(requestFailedException);

            // when
            ValueTask getDocumentTask = this.documentService.RemoveDocumentByFileNameAsync(randomFileName, isDecrypted);

            var actualDependencyException =
                 await Assert.ThrowsAsync<DocumentDependencyException>(getDocumentTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(expectedDependencyException);

            this.blobStorageBrokerMock.Verify(broker =>
                 broker.DeleteFileAsync(randomDocument.FileName, blobContainerName),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDependencyException))),
                         Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteFileIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string randomFileName = GetRandomString();
            var randomMessage = GetRandomString();
            var isDecrypted = false;

            var blobContainerName = this.inMemoryConfiguration
                .GetValue<string>("blobStorage:encryptedBlobContainerName");

            Document randomDocument = new Document
            {
                FileName = randomFileName,
                DocumentData = Encoding.ASCII.GetBytes(GetRandomString())
            };

            var serviceException = new Exception(randomMessage);
            var failedDocumentServiceException = new FailedDocumentServiceException(serviceException);

            var expectedDocumentServiceException =
                new DocumentServiceException(failedDocumentServiceException);

            this.blobStorageBrokerMock.Setup(broker =>
                 broker.DeleteFileAsync(randomDocument.FileName, blobContainerName))
                     .Throws(failedDocumentServiceException);

            // when
            ValueTask getDocumentTask =
                this.documentService.RemoveDocumentByFileNameAsync(randomDocument.FileName, isDecrypted);

            var actualServiceException =
                 await Assert.ThrowsAsync<DocumentServiceException>(getDocumentTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(expectedDocumentServiceException);

            this.blobStorageBrokerMock.Verify(broker =>
                 broker.DeleteFileAsync(randomDocument.FileName, blobContainerName),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDocumentServiceException))),
                         Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
