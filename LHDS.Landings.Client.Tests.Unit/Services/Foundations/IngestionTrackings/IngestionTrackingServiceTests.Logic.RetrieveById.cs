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
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveIngestionTrackingByIdAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedIngestionTracking = storageIngestionTracking.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.ReadIngestionTrackingByIdAsync(inputIngestionTracking.Id))
                    .ReturnsAsync(storageIngestionTracking);

            // when
            IngestionTracking actualIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id);

            // then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.storageBrokerMock.Verify(broker =>
                broker.ReadIngestionTrackingByIdAsync(inputIngestionTracking.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
