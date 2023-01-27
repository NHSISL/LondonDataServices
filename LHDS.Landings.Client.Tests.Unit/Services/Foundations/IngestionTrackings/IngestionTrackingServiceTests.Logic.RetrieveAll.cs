// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Landings.Client.Models.Foundations.IngestionTracking;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public void ShouldReturnIngestionTrackings()
        {
            // given
            IQueryable<IngestionTracking> randomIngestionTracking = CreateRandomIngestionTrackings();
            IQueryable<IngestionTracking> storageIngestionTracking = randomIngestionTracking;
            IQueryable<IngestionTracking> expectedIngestionTracking = storageIngestionTracking;

            this.storageBrokerMock.Setup(broker =>
                broker.ReadAllIngestionTracking())
                    .Returns(storageIngestionTracking);

            // when
            IQueryable<IngestionTracking> actualIngestionTracking =
                this.ingestionTrackingService.RetrieveAllIngestionTracking();

            // then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.storageBrokerMock.Verify(broker =>
                broker.ReadAllIngestionTracking(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
