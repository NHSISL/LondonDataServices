// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public void ShouldReturnOptOuts()
        {
            // given
            IQueryable<OptOut> randomOptOuts = CreateRandomOptOuts();
            IQueryable<OptOut> storageOptOuts = randomOptOuts;
            IQueryable<OptOut> expectedOptOuts = storageOptOuts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOuts())
                    .Returns(storageOptOuts);

            // when
            IQueryable<OptOut> actualOptOuts =
                this.optOutService.RetrieveAllOptOuts();

            // then
            actualOptOuts.Should().BeEquivalentTo(expectedOptOuts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOptOuts(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}