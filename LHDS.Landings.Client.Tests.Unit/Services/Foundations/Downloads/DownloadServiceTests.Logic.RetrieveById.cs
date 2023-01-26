using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Landings.Client.Models.Downloads;
using Xunit;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveDownloadByIdAsync()
        {
            // given
            Download randomDownload = CreateRandomDownload();
            Download inputDownload = randomDownload;
            Download storageDownload = randomDownload;
            Download expectedDownload = storageDownload.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDownloadByIdAsync(inputDownload.Id))
                    .ReturnsAsync(storageDownload);

            // when
            Download actualDownload =
                await this.downloadService.RetrieveDownloadByIdAsync(inputDownload.Id);

            // then
            actualDownload.Should().BeEquivalentTo(expectedDownload);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDownloadByIdAsync(inputDownload.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}