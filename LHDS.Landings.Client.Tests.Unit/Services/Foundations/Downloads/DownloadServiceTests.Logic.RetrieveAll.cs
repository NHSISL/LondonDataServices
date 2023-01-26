using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Landings.Client.Models.Downloads;
using Xunit;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
    {
        [Fact]
        public void ShouldReturnDownloads()
        {
            // given
            IQueryable<Download> randomDownloads = CreateRandomDownloads();
            IQueryable<Download> storageDownloads = randomDownloads;
            IQueryable<Download> expectedDownloads = storageDownloads;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDownloads())
                    .Returns(storageDownloads);

            // when
            IQueryable<Download> actualDownloads =
                this.downloadService.RetrieveAllDownloads();

            // then
            actualDownloads.Should().BeEquivalentTo(expectedDownloads);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDownloads(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}