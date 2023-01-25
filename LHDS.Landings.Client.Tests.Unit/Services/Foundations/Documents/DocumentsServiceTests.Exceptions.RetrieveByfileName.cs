// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowDependencyExceptionOnSelectFileAndLogItAsync()
        {
            // given
            var randomFileName = GetRandomString();

            Document randomDocument = new Document
            {
                FileName = randomFileName,
                DocumentData = Encoding.ASCII.GetBytes(GetRandomString())
            };

            var blobContainerName = this.inMemoryConfiguration.GetValue<string>("blobContainerName");

            var randomMessage = GetRandomString();
            var requestFailedException = new RequestFailedException(randomMessage);

            var failedDocumentRequestException =
                new FailedDocumentRequestException(requestFailedException);

            var expectedDependencyException =
                 new DocumentDependencyException(failedDocumentRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                 broker.GetCurrentDateTimeOffset())
                    .Throws(requestFailedException);

            // when
            ValueTask<Document> getDownloadLinkTask = this.documentService.RetrieveDocumentByFileNameAsync(randomDocument.FileName);

            var actualDependencyException =
                 await Assert.ThrowsAsync<DocumentDependencyException>(getDownloadLinkTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(expectedDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                 broker.GetCurrentDateTimeOffset(),
                     Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                 broker.SelectByFileNameAsync(randomDocument.FileName, blobContainerName),
                     Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDependencyException))),
                         Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}