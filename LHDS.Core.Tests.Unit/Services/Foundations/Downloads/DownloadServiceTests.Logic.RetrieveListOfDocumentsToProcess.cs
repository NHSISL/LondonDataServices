// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveListOfDocumentsToProcess()
        {
            // given
            int randomNumber = GetRandomNumber();
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            List<string> externalDownloadList = GetRandomStrings(randomNumber);
            List<string> expectedDownloadList = externalDownloadList;

            this.downloadBrokerMock.Setup(broker =>
                broker.GetListOfDownloadsToProcessAsync(inputDownload))
                    .ReturnsAsync(externalDownloadList);

            // when
            List<string> actualDownloadList =
                await this.downloadService.RetrieveListOfDocumentsToProcessAsync(inputDownload);

            // then
            actualDownloadList.Should().BeEquivalentTo(expectedDownloadList);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetListOfDownloadsToProcessAsync(inputDownload),
                    Times.Once);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}