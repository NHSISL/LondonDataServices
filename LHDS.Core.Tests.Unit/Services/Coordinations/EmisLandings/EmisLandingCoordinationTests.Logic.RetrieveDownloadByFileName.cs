// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
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
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Document randomDocument = CreateRandomDocument();

            Download inputDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = new Document { FileName = randomDocument.FileName }
            };

            Document inputDocument = randomDocument;
            Document storageDocument = randomDocument;

            Download storageDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = storageDocument
            };

            Download expectedDownload = storageDownload.DeepClone();

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(inputDownload))
                    .ReturnsAsync(storageDownload);

            // when
            Download actualDownload =
                await this.emisLandingCoordinationService.RetrieveDownloadByFileNameAsync(inputDownload);

            // then
            actualDownload.Should().BeEquivalentTo(expectedDownload);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDownloadByFileNameAsync(inputDownload),
                    Times.Once);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

