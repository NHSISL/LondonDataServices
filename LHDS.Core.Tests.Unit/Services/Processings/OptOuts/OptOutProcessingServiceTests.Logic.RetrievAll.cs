// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveOptOutProcessingAllAsync()
        {
            // given
            IQueryable<OptOut> randomOptOuts = CreateRandomOptOuts();
            IQueryable<OptOut> storageOptOuts = randomOptOuts;
            IQueryable<OptOut> expectedOptOuts = storageOptOuts;

            // when
            OptOut actualOptOut =
                await this.optOutProcessingService.RetrieveAllOptOutsAsync();

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOuts);


            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}