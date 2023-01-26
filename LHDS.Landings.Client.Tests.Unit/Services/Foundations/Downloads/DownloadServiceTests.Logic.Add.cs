using System;
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
        public async Task ShouldAddDownloadAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Download randomDownload = CreateRandomDownload(randomDateTimeOffset);
            Download inputDownload = randomDownload;
            Download storageDownload = inputDownload;
            Download expectedDownload = storageDownload.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDownloadAsync(inputDownload))
                    .ReturnsAsync(storageDownload);

            // when
            Download actualDownload = await this.downloadService
                .AddDownloadAsync(inputDownload);

            // then
            actualDownload.Should().BeEquivalentTo(expectedDownload);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDownloadAsync(inputDownload),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}