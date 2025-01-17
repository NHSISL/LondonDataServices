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
        public async Task ShouldRetrieveOptOutProcessingAllAsync()
        {
            // given
            IQueryable<OptOut> randomOptOuts = CreateRandomOptOuts();
            IQueryable<OptOut> retrievedOptOuts = randomOptOuts.DeepClone();

            List<OptOut> expectedOptOuts = retrievedOptOuts.ToList();

            optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ReturnsAsync(retrievedOptOuts);

            // when
            IQueryable<OptOut> actualOptOuts =
                await optOutProcessingService.RetrieveAllOptOutsAsync();

            // then
            actualOptOuts.Should().BeEquivalentTo(expectedOptOuts);

            optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Once);

            optOutServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}