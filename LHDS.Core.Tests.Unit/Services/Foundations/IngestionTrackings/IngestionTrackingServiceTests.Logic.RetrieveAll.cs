// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public void ShouldReturnIngestionTrackings()
        {
            // given
            IQueryable<IngestionTracking> randomIngestionTrackings = CreateRandomIngestionTrackings();
            IQueryable<IngestionTracking> storageIngestionTrackings = randomIngestionTrackings;
            IQueryable<IngestionTracking> expectedIngestionTrackings = storageIngestionTrackings;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackings())
                    .Returns(storageIngestionTrackings);

            // when
            IQueryable<IngestionTracking> actualIngestionTrackings =
                this.ingestionTrackingService.RetrieveAllIngestionTrackings();

            // then
            actualIngestionTrackings.Should().BeEquivalentTo(expectedIngestionTrackings);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackings(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}