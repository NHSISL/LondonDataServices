// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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
        public Task ShouldRetrieveOptOutProcessingAllAsync()
        {
            // given
            IQueryable<OptOut> randomOptOuts = CreateRandomOptOuts();
            IQueryable<OptOut> retrievedOptOuts = randomOptOuts.DeepClone();

            List<OptOut> expectedOptOuts = retrievedOptOuts.ToList();

            optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOuts())
                    .Returns(retrievedOptOuts);

            // when
            IQueryable<OptOut> actualOptOuts =
                optOutProcessingService.RetrieveAllOptOutsAsync();

            // then
            actualOptOuts.Should().BeEquivalentTo(expectedOptOuts);

            optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Once);

            optOutServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
            return Task.CompletedTask;
        }
    }
}