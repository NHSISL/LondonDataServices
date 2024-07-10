// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
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

            string randomData = GetRandomString();
            Stream downloadStream = new MemoryStream(Encoding.UTF8.GetBytes(randomData));
            string expectedData = randomData;

            this.downloadBrokerMock.Setup(broker =>
                broker.GetDownloadByFileNameAsync(download))
                    .Callback<Download>(download =>
                        {
                            downloadStream.Position = 0;
                            downloadStream.CopyTo(download.Document.DocumentData);
                        })
                    .Returns(ValueTask.CompletedTask);

            // when
            await this.downloadService.RetrieveDownloadByFileNameAsync(download);

            // then
            string actualData = Encoding.UTF8.GetString(
                ReadAllBytesFromStream(download.Document.DocumentData));

            actualData.Should().BeEquivalentTo(expectedData);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDownloadByFileNameAsync(download),
                    Times.Once);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}