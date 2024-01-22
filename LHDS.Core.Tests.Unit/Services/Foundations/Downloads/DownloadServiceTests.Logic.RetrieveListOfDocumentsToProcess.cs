// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
    {
        [Fact]
        public async Task ShouldReturnDownloads()
        {
            // given
            List<Document> randomDownloads = CreateRandomDocuments();
            List<Document> externalDownloads = randomDownloads;
            List<Document> expectedDownloads = externalDownloads;

            this.downloadBrokerMock.Setup(broker =>
                broker.GetListOfDocumentsToProcessAsync())
                    .ReturnsAsync(externalDownloads);

            // when
            List<Document> actualDownloads =
                await this.downloadService.RetrieveListOfDocumentsToProcessAsync();

            // then
            actualDownloads.Should().BeEquivalentTo(expectedDownloads);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetListOfDocumentsToProcessAsync(),
                    Times.Once);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}