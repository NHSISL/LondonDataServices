// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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
            string randomContainer = GetRandomString();
            string randomFileName = GetRandomString();
            byte[] randomfileData = Encoding.ASCII.GetBytes(GetRandomString());
            byte[] expectedData = randomfileData;
            Stream randomStream = new MemoryStream();
            Stream outputStream = randomStream;

            this.documentServiceMock
                .Setup(service => service
                    .RetrieveDocumentByFileNameAsync(randomStream, randomFileName, randomContainer))
                .Callback<Stream, string, string>((output, fileName, container) =>
                {
                    output = new MemoryStream(randomfileData);
                })
                .Returns(ValueTask.CompletedTask);

            // When
            await this.documentProcessingService
                .RetrieveDocumentByFileNameAsync(
                    output: outputStream,
                    fileName: randomFileName,
                    container: randomContainer);

            // Then
            byte[] actualData = ReadAllBytesFromStream(outputStream);
            actualData.Should().BeEquivalentTo(expectedData);

            this.documentServiceMock.Verify(service =>
                service.RetrieveDocumentByFileNameAsync(
                    It.Is(SameStreamAs(new MemoryStream())), randomFileName, randomContainer),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}