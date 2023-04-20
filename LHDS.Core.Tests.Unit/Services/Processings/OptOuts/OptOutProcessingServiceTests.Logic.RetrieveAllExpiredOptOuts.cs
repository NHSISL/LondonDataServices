// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldRetrieveAllExpiredOptoutsAsync()
        {
            // given
            int olderThanDays = GetRandomNumber();
            List<OptOut> randomOptOuts = CreateRandomOptOuts();
            List<OptOut> retrievedOptOuts = randomOptOuts.DeepClone();
            List<OptOut> expectedOptOuts = new List<OptOut>();
            DateTime today = DateTime.UtcNow;

            foreach (OptOut optOut in randomOptOuts)
            {
                if (optOut.CreatedDate < today.AddDays(-olderThanDays))
                {
                   expectedOptOuts.Add(optOut);
                }
            }

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOuts())
                    .Returns(retrievedOptOuts.AsQueryable());

            // when
            List<OptOut> actualOptOuts =
                await this.optOutProcessingService.RetrieveAllExpiredOptOutsAsync(olderThanDays);

            // then
            actualOptOuts.Should().BeEquivalentTo(expectedOptOuts);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
