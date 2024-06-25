// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
            List<string> randomList = CreateRandomStringList();
            List<string> externalDownloadList = randomList;
            List<string> expectedDownloadList = externalDownloadList.DeepClone();

            this.downloadServiceMock.Setup(broker =>
                broker.RetrieveListOfDocumentsToProcessAsync(inputDownload))
                    .ReturnsAsync(externalDownloadList);

            // when
            List<string> actualDownloadList =
                await this.downloadProcessingService.RetrieveListOfDownloadsToProcessAsync(inputDownload);

            // then
            actualDownloadList.Should().BeEquivalentTo(expectedDownloadList);

            this.downloadServiceMock.Verify(broker =>
                broker.RetrieveListOfDocumentsToProcessAsync(inputDownload),
                    Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}