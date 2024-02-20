// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Processings.SubscriberCredentials;
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
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            List<Document> randomDocuments = CreateRandomDocuments();

            List<Download> externalDownloads = randomDocuments.Select(document =>
                new Download
                {
                    SubscriberCredential = inputSubscriberCredential,
                    Document = document
                }).ToList();

            List<Download> expectedDownloads = externalDownloads.DeepClone();

            this.downloadServiceMock.Setup(broker =>
                broker.RetrieveListOfDocumentsToProcessAsync(inputDownload))
                    .ReturnsAsync(externalDownloads);

            // when
            List<Download> actualDownloads =
                await this.downloadProcessingService.RetrieveListOfDownloadsToProcessAsync(inputDownload);

            // then
            actualDownloads.Should().BeEquivalentTo(expectedDownloads);

            this.downloadServiceMock.Verify(broker =>
                broker.RetrieveListOfDocumentsToProcessAsync(inputDownload),
                    Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}