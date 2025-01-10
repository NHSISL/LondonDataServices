// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
            int olderThanDays = GetRandomValidExpiryDays(7);
            DateTimeOffset currentDate = GetRandomDateTimeOffset();
            DateTimeOffset expireDate = currentDate.AddDays(-olderThanDays);
            IQueryable<OptOut> randomOptOuts = CreateRandomOptOuts(expireDate);
            IQueryable<OptOut> retrievedOptOuts = randomOptOuts.DeepClone();

            List<OptOut> expectedOptOuts = retrievedOptOuts
                .Where(optOut => optOut.CacheTime < expireDate)
                    .ToList();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDate);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                        .ReturnsAsync(retrievedOptOuts);

            // when
            List<OptOut> actualOptOuts =
                await this.optOutProcessingService.RetrieveAllExpiredOptOutsAsync(olderThanDays);

            // then
            actualOptOuts.Count().Should().Be(3);
            actualOptOuts.Should().BeEquivalentTo(expectedOptOuts);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
