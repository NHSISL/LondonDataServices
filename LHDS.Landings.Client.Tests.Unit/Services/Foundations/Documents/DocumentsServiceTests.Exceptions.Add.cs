// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DocumentsServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnUploadFileAndLogItAsync()
        {
            // given
            var blobContainerName = this.inMemoryConfiguration.GetValue<string>("blobContainerName");
            var randomString = GetRandomString();
            var randomStream = new MemoryStream(Encoding.ASCII.GetBytes(GetRandomString()));
            var randomMessage = GetRandomString();

            Document document = new Document
            {
                FileName = randomString,
                DocumentStream = randomStream
            };

            var requestFailedException = new RequestFailedException(randomMessage);
            var failedDocumentRequestException = new FailedDocumentRequestException(requestFailedException);

            var expectedDependencyException =
                 new DocumentDependencyException(failedDocumentRequestException);

            this.blobStorageBrokerMock.Setup(broker =>
                 broker.InsertFileAsync(document.FileName, document.DocumentStream, blobContainerName))
                    .Throws(requestFailedException);

            // when
            ValueTask uploadFileTask = this.documentService.AddDocumentAsync(document);

            var actualDependencyException =
                 await Assert.ThrowsAsync<DocumentDependencyException>(uploadFileTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(expectedDependencyException);

            this.blobStorageBrokerMock.Verify(broker =>
                 broker.InsertFileAsync(document.FileName, document.DocumentStream, blobContainerName),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDependencyException))),
                         Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnUploadFileIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var blobContainerName = this.inMemoryConfiguration.GetValue<string>("blobContainerName");
            var randomString = GetRandomString();
            var randomStream = new MemoryStream(Encoding.ASCII.GetBytes(GetRandomString()));
            var randomMessage = GetRandomString();

            Document document = new Document
            {
                FileName = randomString,
                DocumentStream = randomStream
            };

            var serviceException = new Exception(randomMessage);
            var failedDocumentServiceException = new FailedDocumentServiceException(serviceException);

            var expectedDocumentServiceException =
                new DocumentServiceException(failedDocumentServiceException);

            this.blobStorageBrokerMock.Setup(broker =>
                 broker.InsertFileAsync(document.FileName, document.DocumentStream, blobContainerName))
                     .Throws(failedDocumentServiceException);

            // when
            ValueTask uploadFileTask = this.documentService.AddDocumentAsync(document);

            var actualServiceException =
                 await Assert.ThrowsAsync<DocumentServiceException>(uploadFileTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(expectedDocumentServiceException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(document.FileName, document.DocumentStream, blobContainerName),
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