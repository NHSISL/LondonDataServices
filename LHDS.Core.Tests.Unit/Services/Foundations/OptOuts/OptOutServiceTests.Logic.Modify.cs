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
        public async Task ShouldModifyOptOutAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OptOut randomOptOut = CreateRandomModifyOptOut(randomDateTimeOffset);
            OptOut inputOptOut = randomOptOut;
            OptOut storageOptOut = inputOptOut.DeepClone();
            storageOptOut.UpdatedDate = randomOptOut.CreatedDate;
            OptOut updatedOptOut = inputOptOut;
            OptOut expectedOptOut = updatedOptOut.DeepClone();
            Guid optOutId = inputOptOut.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateOptOutAsync(inputOptOut))
                    .ReturnsAsync(updatedOptOut);

            // when
            OptOut actualOptOut =
                await this.optOutService.ModifyOptOutAsync(inputOptOut);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(inputOptOut),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}