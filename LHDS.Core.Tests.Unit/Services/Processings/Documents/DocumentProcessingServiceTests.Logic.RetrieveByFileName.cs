// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Documents
{
    public partial class DocumentProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveFileAsync()
        {
            // Given
            var randomFileName = GetRandomString();
            var randomfileData = Encoding.ASCII.GetBytes(GetRandomString());

            Document randomDocument = new Document
            {
                FileName = randomFileName,
                DocumentData = randomfileData
            };

            Document expectedDocument = randomDocument;

            this.documentServiceMock.Setup(service =>
                service.RetrieveDocumentByFileNameAsync(randomDocument.FileName))
                    .ReturnsAsync(randomDocument);

            // When
            Document actualDocument =
                await this.documentProcessingService
                    .RetrieveDocumentByFileNameAsync(randomDocument.FileName);

            // Then
            actualDocument.Should().BeEquivalentTo(expectedDocument);

            this.documentServiceMock.Verify(service =>
                service.RetrieveDocumentByFileNameAsync(randomDocument.FileName),
                    Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}