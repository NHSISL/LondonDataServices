// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllIngestionTrackingsAsync()
        {
            // given
            List<IngestionTracking> randomIngestionTrackings = CreateRandomIngestionTrackings();
            List<IngestionTracking> storageIngestionTrackings = randomIngestionTrackings;
            IQueryable<IngestionTracking> expectedIngestionTrackings = storageIngestionTrackings.AsQueryable();

            this.ingestionTrackingServiceMock.Setup(broker =>
                broker.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(storageIngestionTrackings.AsQueryable());

            // when
            IQueryable<IngestionTracking> actualIngestionTrackings =
                await this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackingsAsync();

            // then
            actualIngestionTrackings.Should().BeEquivalentTo(expectedIngestionTrackings);

            this.ingestionTrackingServiceMock.Verify(broker =>
                broker.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
