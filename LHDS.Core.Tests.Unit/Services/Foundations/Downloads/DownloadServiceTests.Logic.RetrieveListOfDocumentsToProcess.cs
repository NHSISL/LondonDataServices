// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Processings.SubscriberCredentials;
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
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            List<Document> randomDowcuments = CreateRandomDocuments();
            List<Document> externalDocuments = randomDowcuments;

            List<Download> externalDownloads = externalDocuments.Select(document =>
                new Download
                {
                    SubscriberCredential = inputSubscriberCredential,
                    Document = document
                }).ToList();

            List<Download> expectedDownloads = externalDownloads;

            this.downloadBrokerMock.Setup(broker =>
                broker.GetListOfDownloadsToProcessAsync(inputDownload))
                    .ReturnsAsync(externalDownloads);

            // when
            List<Download> actualDownloads =
                await this.downloadService.RetrieveListOfDocumentsToProcessAsync(inputDownload);

            // then
            actualDownloads.Should().BeEquivalentTo(expectedDownloads);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetListOfDownloadsToProcessAsync(inputDownload),
                    Times.Once);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}