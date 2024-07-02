// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldRetrieveDownloadByFileNameAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string fileName = randomFileName;
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            byte[] randomoutputData = CreateRandomData();
            byte[] expectedData = randomoutputData.DeepClone();
            Stream initialOutputStream = new MemoryStream();
            Stream outputStream = new MemoryStream();
            Stream returnedOutputStream = new MemoryStream(randomoutputData);


            Download inputDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = new Document
                {
                    FileName = fileName,
                    DocumentData = outputStream
                }
            };

            byte[] expectedDownload = randomoutputData.DeepClone();

            this.downloadProcessingServiceMock
                .Setup(service =>
                    service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))))
                .Callback<Download>(download =>
                    {
                        returnedOutputStream.Position = 0;
                        returnedOutputStream.CopyTo(outputStream);
                    })
                .Returns(ValueTask.CompletedTask);

            // when
            await this.emisLandingOrchestrationService
                .RetrieveDownloadByFileNameAsync(
                    output: outputStream,
                    fileName: fileName,
                    subscriberCredential: inputSubscriberCredential);

            byte[] actualData = ReadAllBytesFromStream(outputStream);

            // then
            actualData.Should().BeEquivalentTo(expectedData);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))),
                    Times.Once);

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
        }
    }
}