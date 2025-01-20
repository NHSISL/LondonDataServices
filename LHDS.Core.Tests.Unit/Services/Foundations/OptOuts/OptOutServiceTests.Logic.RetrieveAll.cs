// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldReturnOptOutsAsync()
        {
            // given
            IQueryable<OptOut> randomOptOuts = CreateRandomOptOuts();
            IQueryable<OptOut> storageOptOuts = randomOptOuts;
            IQueryable<OptOut> expectedOptOuts = storageOptOuts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(storageOptOuts);

            // when
            IQueryable<OptOut> actualOptOuts =
                await this.optOutService.RetrieveAllOptOutsAsync();

            // then
            actualOptOuts.Should().BeEquivalentTo(expectedOptOuts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOptOutsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}