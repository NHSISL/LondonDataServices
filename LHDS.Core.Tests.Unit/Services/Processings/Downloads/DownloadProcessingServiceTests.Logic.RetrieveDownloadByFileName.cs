// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
            Document randomDocument = CreateRandomDocument();

            Download inputDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = new Document { FileName = randomDocument.FileName }
            };

            Download storageDownload = new Download
            {
                Document = randomDocument,
                SubscriberCredential = inputSubscriberCredential
            };

            Document storageDocument = randomDocument;
            Document expectedDocument = storageDocument.DeepClone();

            this.downloadServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(inputDownload))
                    .ReturnsAsync(storageDownload);

            // when
            Download actualDocument =
                await this.downloadProcessingService.RetrieveDownloadByFileNameAsync(inputDownload);

            // then
            actualDocument.Should().BeEquivalentTo(expectedDocument);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(inputDownload),
                    Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}