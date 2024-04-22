// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveOptOutByIdAsync()
        {
            // given
            OptOut randomOptOut = CreateRandomOptOut();
            OptOut inputOptOut = randomOptOut;
            OptOut storageOptOut = randomOptOut;
            OptOut expectedOptOut = storageOptOut.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(inputOptOut.Id))
                    .ReturnsAsync(storageOptOut);

            // when
            OptOut actualOptOut =
                await this.optOutService.RetrieveOptOutByIdAsync(inputOptOut.Id);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(inputOptOut.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}