// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllIngestionTrackings()
        {
            // given
            List<IngestionTracking> randomIngestionTrackings = CreateRandomIngestionTrackings();
            List<IngestionTracking> storageIngestionTrackings = randomIngestionTrackings;
            IQueryable<IngestionTracking> expectedIngestionTrackings = storageIngestionTrackings.AsQueryable();

            this.ingestionTrackingServiceMock.Setup(broker =>
                broker.RetrieveAllIngestionTrackings())
                    .Returns(storageIngestionTrackings.AsQueryable());

            // when
            IQueryable<IngestionTracking> actualIngestionTrackings =
                this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings();

            // then
            actualIngestionTrackings.Should().BeEquivalentTo(expectedIngestionTrackings);

            this.ingestionTrackingServiceMock.Verify(broker =>
                broker.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
