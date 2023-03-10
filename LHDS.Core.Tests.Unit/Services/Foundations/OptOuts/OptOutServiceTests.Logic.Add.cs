using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.OptOuts;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldAddOptOutAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            OptOut randomOptOut = CreateRandomOptOut(randomDateTimeOffset);
            OptOut inputOptOut = randomOptOut;
            OptOut storageOptOut = inputOptOut;
            OptOut expectedOptOut = storageOptOut.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertOptOutAsync(inputOptOut))
                    .ReturnsAsync(storageOptOut);

            // when
            OptOut actualOptOut = await this.optOutService
                .AddOptOutAsync(inputOptOut);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(inputOptOut),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}