// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
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
        public async Task ShouldRetrieveDownloadByFileNameAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            string fileName = GetRandomString();

            Download download = new Download
            {
                SubscriberCredential = inputSubscriberCredential,

                Document = new Document
                {
                    FileName = fileName,
                    DocumentData = new MemoryStream()
                }
            };

            Download initialDownload = download.DeepClone();
            string randomData = GetRandomString();
            string expectedData = randomData;
            Stream randomStream = new MemoryStream(Encoding.UTF8.GetBytes(randomData));
            Stream downloadedStream = randomStream;

            this.downloadServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(download))
                    .Callback<Download>(download =>
                    {
                        downloadedStream.Position = 0;
                        downloadedStream.CopyTo(download.Document.DocumentData);
                    })
                    .Returns(ValueTask.CompletedTask);

            // when
            await this.downloadProcessingService.RetrieveDownloadByFileNameAsync(download);

            // then
            string actualData = Encoding.UTF8.GetString(
                ReadAllBytesFromStream(download.Document.DocumentData));

            actualData.Should().BeEquivalentTo(expectedData);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(initialDownload))),
                    Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}