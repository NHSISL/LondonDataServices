// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact(Skip = "Conversion to stream")]
        public async Task ShouldRetrieveDownloadByFileNameAsync()
        {
            //// given
            //string randomFileName = GetRandomString();
            //SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            //SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            //Document randomDocument = CreateRandomDocument();
            //Document externalDocument = randomDocument;

            //Download inputDownload = new Download
            //{
            //    SubscriberCredential = inputSubscriberCredential,
            //    Document = new Document { FileName = externalDocument.FileName }
            //};

            //Download storageDownload = new Download
            //{
            //    SubscriberCredential = inputSubscriberCredential,
            //    Document = externalDocument
            //};

            //byte[] expectedDownload = storageDownload.Document.DocumentData.DeepClone();

            //this.downloadProcessingServiceMock.Setup(service =>
            //        service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))))
            //            .ReturnsAsync(storageDownload);

            //// when
            //byte[] actualDownload = await this.emisLandingOrchestrationService
            //    .RetrieveDownloadByFileNameAsync(
            //        fileName: externalDocument.FileName,
            //        subscriberCredential: inputSubscriberCredential);

            //// then
            //actualDownload.Should().BeEquivalentTo(expectedDownload);

            //this.downloadProcessingServiceMock.Verify(service =>
            //    service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))),
            //        Times.Once);

            //this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            //this.dateTimeBrokerMock.VerifyNoOtherCalls();
            //this.hashBrokerMock.VerifyNoOtherCalls();
            //this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            //this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            //this.documentProcessingServiceMock.VerifyNoOtherCalls();
            //this.loggingBrokerMock.VerifyNoOtherCalls();
            //this.auditServiceMock.VerifyNoOtherCalls();
        }
    }
}