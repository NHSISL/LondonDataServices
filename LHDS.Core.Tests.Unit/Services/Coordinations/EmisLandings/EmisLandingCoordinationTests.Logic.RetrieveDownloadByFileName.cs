// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveDownloadByFileNameAndLogAsync()
        {
            // given
            Guid subscriberCredentailId = Guid.NewGuid();
            string outputFileName = CreateRandomSubscriberCredentialIdFileName(subscriberCredentailId);
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberCredentailId);
            SubscriberCredential storageSubscriberCredential = randomSubscriberCredential;
            Stream downloadStream = new MemoryStream(CreateRandomData());
            Stream expectedStream = downloadStream;
            Stream outputStream = new MemoryStream();
            Stream actualStream = outputStream;

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(subscriberCredentailId, false)).
                    ReturnsAsync(storageSubscriberCredential);

            this.emisLandingOrchestrationServiceMock
                .Setup(service => service.RetrieveDownloadByFileNameAsync(
                    It.Is(SameStreamAs(outputStream)),
                    outputFileName,
                    It.Is(SameSubscriberCredentialAs(storageSubscriberCredential))))
                .Callback<Stream, string, SubscriberCredential>((output, fileName, subscriberCredential) =>
                {
                    downloadStream.Position = 0;
                    downloadStream.CopyTo(outputStream);
                })
                .Returns(ValueTask.CompletedTask);

            // when
            await this.emisLandingCoordinationService
                .RetrieveDownloadByFileNameAsync(output: new MemoryStream(), outputFileName);

            // then
            Assert.True(IsSameStream(expectedStream, actualStream));

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(subscriberCredentailId, false),
                    Times.Once);

            this.emisLandingOrchestrationServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(
                    It.IsAny<Stream>(),
                    outputFileName,
                    It.Is(SameSubscriberCredentialAs(storageSubscriberCredential))),
                        Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

