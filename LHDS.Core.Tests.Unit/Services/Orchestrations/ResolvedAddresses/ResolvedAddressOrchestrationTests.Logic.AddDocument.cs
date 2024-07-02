// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact]
        public async Task ShouldAddFileAsync()
        {
            // Given
            var inputContainer = GetRandomString();
            var inputFileName = GetRandomString();
            var inputFileData = Encoding.UTF8.GetBytes(GetRandomString());
            Stream inputStream = new MemoryStream(inputFileData);
            Stream expectedStream = inputStream;
            Stream actualStream = new MemoryStream();

            this.documentProcessingServiceMock
                .Setup(service => service.AddDocumentAsync(It.IsAny<Stream>(), inputFileName, inputContainer))
                .Callback<Stream, string, string>((input, fileName, container) =>
                {
                    input.Position = 0;
                    input.CopyTo(actualStream);
                })
                .Returns(ValueTask.CompletedTask);

            // When
            await this.resolvedAddressOrchestrationService
                .AddDocumentAsync(data: inputFileData, fileName: inputFileName, container: inputContainer);

            // Then
            Assert.True(IsSameStream(expectedStream, actualStream));

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), inputFileName, inputContainer),
                    Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
