// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldAddFileAsync()
        {
            // Given
            var randomContainer = GetRandomString();
            var randomFileName = GetRandomString();
            var randomfileData = Encoding.ASCII.GetBytes(GetRandomString());

            Document document = new Document
            {
                FileName = randomFileName,
                DocumentData = randomfileData
            };

            string expectedDocumentFileName = document.FileName;

            // When
            string actualDocumentFileName = await this.documentProcessingService
                .AddDocumentAsync(document, container: randomContainer);

            // Then
            actualDocumentFileName.Should().BeEquivalentTo(expectedDocumentFileName);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(It.Is<Document>(doc =>
                    doc.FileName == randomFileName && doc.DocumentData == randomfileData), randomContainer),
                    Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}