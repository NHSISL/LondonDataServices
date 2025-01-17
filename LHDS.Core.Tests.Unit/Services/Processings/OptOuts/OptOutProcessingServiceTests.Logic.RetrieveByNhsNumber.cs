// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveOptOutProcessingByNhsNumberAsync()
        {
            // given
            OptOut randomOptOut = CreateRandomOptOut();
            List<OptOut> retrievedOptOut = new List<OptOut> { randomOptOut };
            OptOut expectedOptOut = randomOptOut.DeepClone();

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ReturnsAsync(retrievedOptOut.AsQueryable());

            // when
            OptOut actualOptOut =
                await this.optOutProcessingService.RetrieveOptOutByNhsNumberAsync(randomOptOut.NhsNumber);

            // then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}