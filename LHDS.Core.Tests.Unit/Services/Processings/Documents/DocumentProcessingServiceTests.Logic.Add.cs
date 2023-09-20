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
        public async Task ShouldAddDataSetAsync()
        {
            // Given
            var randomFileName = GetRandomString();
            var randomfileData = Encoding.ASCII.GetBytes(GetRandomString());

            Document document = new Document
            {
                FileName = randomFileName,
                DocumentData = randomfileData
            };

            // When
            await this.documentProcessingService.AddDocumentAsync(document);

            // Then
            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(It.Is<Document>(doc =>
                    doc.FileName == randomFileName && doc.DocumentData == randomfileData)),
                    Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}