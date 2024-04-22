// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveIngestionTrackingByFileNameAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedIngestionTracking = storageIngestionTracking.DeepClone();

            IQueryable<IngestionTracking> storageIngestionTrackings =
                new List<IngestionTracking> { storageIngestionTracking }.AsQueryable();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackings())
                    .Returns(storageIngestionTrackings);

            // when
            IngestionTracking actualIngestionTracking =
                await this.ingestionTrackingService
                    .RetrieveIngestionTrackingByFileNameAsync(inputIngestionTracking.FileName);

            // then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackings(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}