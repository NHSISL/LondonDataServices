// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Documents
{
    public partial class DocumentProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveDocumentProcessingDownloadlinkAsync()
        {
            // Given
            var randomContainer = GetRandomString();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            string randomSasUrl = GetRandomString();
            string outputSasUrl = randomSasUrl;
            string expectedSasUrl = randomSasUrl;

            this.documentServiceMock.Setup(service =>
                service.GetDownloadLinkAsync(inputFileName, randomContainer))
                    .ReturnsAsync(outputSasUrl);

            // When
            string actualSasUrl =
                await this.documentProcessingService
                    .GetDownloadLinkAsync(inputFileName, randomContainer);

            // Then
            actualSasUrl.Should().BeEquivalentTo(expectedSasUrl);

            this.documentServiceMock.Verify(service =>
                service.GetDownloadLinkAsync(inputFileName, randomContainer),
                    Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}