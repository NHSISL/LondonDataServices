// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
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
            string fileName = CreateRandomSubscriberCredentialIdFileName(subscriberCredentailId);
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberCredentailId);
            SubscriberCredential storageSubscriberCredential = randomSubscriberCredential;
            Document randomDocument = CreateRandomDocument();
            randomDocument.FileName = fileName;
            randomDocument.SHA256Hash = string.Empty;
            Document storageDocument = randomDocument.DeepClone();
            Document expectedDocument = storageDocument.DeepClone();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(subscriberCredentailId, false)).
                    ReturnsAsync(storageSubscriberCredential);

            //this.emisLandingOrchestrationServiceMock.Setup(service =>
            //    service.RetrieveDownloadByFileNameAsync(
            //        fileName,
            //        It.Is(SameSubscriberCredentialAs(storageSubscriberCredential))))
            //            .ReturnsAsync(storageDocument.DocumentData);

            // when
            await this.emisLandingCoordinationService
                .RetrieveDownloadByFileNameAsync(output: new MemoryStream(), fileName);

            // then
            Assert.Fail();
            //actualDocument.Should().BeEquivalentTo(expectedDocument);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(subscriberCredentailId, false),
                    Times.Once());

            //this.emisLandingOrchestrationServiceMock.Verify(service =>
            //    service.RetrieveDownloadByFileNameAsync(
            //        fileName,
            //        It.Is(SameSubscriberCredentialAs(storageSubscriberCredential))),
            //            Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

