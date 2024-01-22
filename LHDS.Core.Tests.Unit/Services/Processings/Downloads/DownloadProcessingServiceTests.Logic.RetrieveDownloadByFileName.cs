// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
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
            Document randomDocument = CreateRandomDocument();
            Document inputDocument = randomDocument;
            Document storageDocument = randomDocument;
            Document expectedDocument = storageDocument.DeepClone();

            this.downloadServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(inputDocument.FileName))
                    .ReturnsAsync(storageDocument);

            // when
            Document actualDocument =
                await this.downloadProcessingService.RetrieveDownloadByFileNameAsync(inputDocument.FileName);

            // then
            actualDocument.Should().BeEquivalentTo(expectedDocument);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(inputDocument.FileName),
                    Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}