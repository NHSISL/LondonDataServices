// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
            SubscriberCredential randomSubscriberCredential = new SubscriberCredential { Id = subscriberCredentailId }; ;
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Document randomDocument = CreateRandomDocument();
            randomDocument.FileName = fileName;
            randomDocument.SHA256Hash = string.Empty;
            Document storageDocument = randomDocument.DeepClone();
            Document expectedDocument = storageDocument.DeepClone();

            this.emisLandingOrchestrationServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(
                    fileName,
                    It.Is(SameSubscriberCredentialAs(inputSubscriberCredential))))
                        .ReturnsAsync(storageDocument.DocumentData);

            // when
            Document actualDocument =
                await this.emisLandingCoordinationService.RetrieveDownloadByFileNameAsync(fileName);

            // then
            actualDocument.Should().BeEquivalentTo(expectedDocument);

            this.emisLandingOrchestrationServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(
                    fileName,
                    It.Is(SameSubscriberCredentialAs(inputSubscriberCredential))),
                        Times.Once);

            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

