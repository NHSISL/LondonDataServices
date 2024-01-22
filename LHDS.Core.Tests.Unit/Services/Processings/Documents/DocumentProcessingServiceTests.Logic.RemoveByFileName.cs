// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Documents
{
    public partial class DocumentProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRemoveDocumentAsync()
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

            // When
            await this.documentProcessingService
                .RemoveDocumentByFileNameAsync(fileName: document.FileName, container: randomContainer);

            // Then
            this.documentServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(document.FileName, randomContainer),
                    Times.Once);
        }
    }
}