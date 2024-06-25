// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
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

            // When
            await this.documentProcessingService
                .RemoveDocumentByFileNameAsync(fileName: randomFileName, container: randomContainer);

            // Then
            this.documentServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(randomFileName, randomContainer),
                    Times.Once);
        }
    }
}