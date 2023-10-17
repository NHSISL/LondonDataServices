// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Downloads
{
    public partial class DownloadProcessingServiceTests
    {
        [Fact]
        public async Task ShouldReturnDownloads()
        {
            // given
            List<Document> randomDownloads = CreateRandomDocuments();
            List<Document> externalDownloads = randomDownloads;
            List<Document> expectedDownloads = externalDownloads;

            this.downloadServiceMock.Setup(broker =>
                broker.RetrieveListOfDocumentsToProcessAsync())
                    .ReturnsAsync(externalDownloads);

            // when
            List<Document> actualDownloads =
                await this.downloadProcessingService.RetrieveListOfDocumentsToProcessAsync();

            // then
            actualDownloads.Should().BeEquivalentTo(expectedDownloads);

            this.downloadServiceMock.Verify(broker =>
                broker.RetrieveListOfDocumentsToProcessAsync(),
                    Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}