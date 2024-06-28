// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading.Tasks;
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
            var randomfileData = Encoding.UTF8.GetBytes(GetRandomString());
            Stream randomStream = new MemoryStream(randomfileData);

            // When
            await this.documentProcessingService.AddDocumentAsync(
                input: randomStream,
                fileName: randomFileName,
                container: randomContainer);

            // Then
            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(randomStream, randomFileName, randomContainer),
                    Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}