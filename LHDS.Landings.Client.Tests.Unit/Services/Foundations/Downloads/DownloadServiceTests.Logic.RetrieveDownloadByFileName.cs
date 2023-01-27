// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Landings.Client.Models.Foundations.Documents;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveDownloadByIdAsync()
        {
            // given
            Document randomDocument = CreateRandomDocument();
            Document inputDocument = randomDocument;
            Document storageDocument = randomDocument;
            Document expectedDocument = storageDocument.DeepClone();

            this.downloadBrokerMock.Setup(broker =>
                broker.GetDocumentByFileNameAsync(inputDocument.FileName))
                    .ReturnsAsync(storageDocument);

            // when
            Document actualDocument =
                await this.downloadService.RetrieveDownloadByFileNameAsync(inputDocument.FileName);

            // then
            actualDocument.Should().BeEquivalentTo(expectedDocument);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDocumentByFileNameAsync(inputDocument.FileName),
                    Times.Once);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}